import { workerData } from 'worker_threads';
import { Semaphore } from './semaphore';
import { IDX } from './manager';

const { producerId, itemsToProduce, totalItems, storageSize, sharedBuffer } = workerData as {
    producerId: number; itemsToProduce: number;
    totalItems: number; storageSize: number;
    sharedBuffer: SharedArrayBuffer;
};

const arr = new Int32Array(sharedBuffer);
const access = new Semaphore(arr, IDX.ACCESS);
const full = new Semaphore(arr, IDX.FULL);
const empty = new Semaphore(arr, IDX.EMPTY);

const sleepBuf = new Int32Array(new SharedArrayBuffer(4));
const sleep = (ms: number) => Atomics.wait(sleepBuf, 0, 0, ms);

let produced = 0;

while (produced < itemsToProduce) {
    const globalCount = Atomics.add(arr, IDX.PRODUCED, 1);
    if (globalCount >= totalItems) { Atomics.sub(arr, IDX.PRODUCED, 1); break; }

    full.acquire();
    access.acquire();

    const writePtr = Atomics.load(arr, IDX.WRITE);
    Atomics.store(arr, IDX.BUFFER + writePtr, globalCount);
    Atomics.store(arr, IDX.WRITE, (writePtr + 1) % storageSize);
    Atomics.add(arr, IDX.SIZE, 1);

    const count = Atomics.load(arr, IDX.SIZE);
    process.stdout.write(`[Producer-${producerId}] Added: P${producerId}-item${produced}  | storage: ${count}\n`);

    access.release();
    empty.release();

    produced++;
    sleep(500);
}

process.stdout.write(`[Producer-${producerId}] Done.\n`);