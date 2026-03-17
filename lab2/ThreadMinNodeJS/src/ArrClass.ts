import { Worker } from 'worker_threads';
import * as path from 'path';

const LOCK = 0;
const GLOBAL_MIN = 1;
const GLOBAL_IDX = 2;
const COUNT = 3;

export class ArrClass {
    private readonly dim: number;
    private readonly threadNum: number;

    public readonly sharedBuffer: SharedArrayBuffer;
    public readonly arr: Int32Array;

    // Спільний буфер синхронізації: [lock, globalMin, globalMinIndex, threadCount]
    private readonly syncBuffer: SharedArrayBuffer;
    private readonly syncArr: Int32Array;

    constructor(dim: number, threadNum: number) {
        this.dim = dim;
        this.threadNum = threadNum;

        this.sharedBuffer = new SharedArrayBuffer(dim * Int32Array.BYTES_PER_ELEMENT);
        this.arr = new Int32Array(this.sharedBuffer);

        // 4 комірки: LOCK, GLOBAL_MIN, GLOBAL_IDX, COUNT
        this.syncBuffer = new SharedArrayBuffer(4 * Int32Array.BYTES_PER_ELEMENT);
        this.syncArr = new Int32Array(this.syncBuffer);

        Atomics.store(this.syncArr, LOCK, 0);                  // лок вільний
        Atomics.store(this.syncArr, GLOBAL_MIN, 2147483647);   // MAX_INT
        Atomics.store(this.syncArr, GLOBAL_IDX, -1);
        Atomics.store(this.syncArr, COUNT, 0);                 // жоден потік не завершив

        for (let i = 0; i < dim; i++) {
            this.arr[i] = Math.floor(Math.random() * 1_000_000) + 1;
        }

        const negIndex = Math.floor(Math.random() * dim);
        this.arr[negIndex] = -(Math.floor(Math.random() * 1_000_000) + 1);
        console.log(`Вставлено від'ємне значення: ${this.arr[negIndex]} за індексом: ${negIndex}`);
    }

    public async threadMin(): Promise<[number, number]> {
        const chunkSize = Math.floor(this.dim / this.threadNum);

        for (let i = 0; i < this.threadNum; i++) {
            const start = i * chunkSize;
            const end = (i === this.threadNum - 1) ? this.dim : start + chunkSize;

            new Worker(path.resolve(__dirname, 'worker.js'), {
                workerData: {
                    threadId: i,
                    startIndex: start,
                    finishIndex: end,
                    sharedBuffer: this.sharedBuffer,
                    syncBuffer: this.syncBuffer,
                }
            });
        }

        let finished = Atomics.load(this.syncArr, COUNT);
        while (finished < this.threadNum) {
            // Atomics.waitAsync — неблокуюча версія wait() для головного потоку
            // Чекаю поки COUNT зміниться від поточного значення
            const result = Atomics.waitAsync(this.syncArr, COUNT, finished);
            if (result.async) {
                await result.value;
            }
            finished = Atomics.load(this.syncArr, COUNT);
        }

        return [
            Atomics.load(this.syncArr, GLOBAL_MIN),
            Atomics.load(this.syncArr, GLOBAL_IDX),
        ];
    }
}
