import { workerData } from 'worker_threads';
import { Semaphore } from './semaphore';
import { IDX } from './manager';

const { consumerId, itemsToConsume, storageSize, sharedBuffer } = workerData as {
    consumerId: number; itemsToConsume: number;
    storageSize: number; sharedBuffer: SharedArrayBuffer;
};

const arr = new Int32Array(sharedBuffer);
const access = new Semaphore(arr, IDX.ACCESS);
const full = new Semaphore(arr, IDX.FULL);
const empty = new Semaphore(arr, IDX.EMPTY);

const sleepBuf = new Int32Array(new SharedArrayBuffer(4));
const sleep = (ms: number) => Atomics.wait(sleepBuf, 0, 0, ms);

for (let i = 0; i < itemsToConsume; i++) {
    empty.acquire();
    access.acquire();

    const readPtr = Atomics.load(arr, IDX.READ);
    const itemNum = Atomics.load(arr, IDX.BUFFER + readPtr);
    Atomics.store(arr, IDX.READ, (readPtr + 1) % storageSize);
    Atomics.sub(arr, IDX.SIZE, 1); // декремент

    const count = Atomics.load(arr, IDX.SIZE);
    process.stdout.write(`[Consumer-${consumerId}] Took: item${itemNum}  | storage: ${count}\n`);

    access.release();
    full.release();

    sleep(800);
}

process.stdout.write(`[Consumer-${consumerId}] Done.\n`);