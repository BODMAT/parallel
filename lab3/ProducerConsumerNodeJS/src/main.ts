import { Worker } from 'worker_threads';
import * as path from 'path';
import { createSharedState } from './manager';

const STORAGE_SIZE = 5;
const TOTAL_ITEMS = 20;
const NUM_PRODUCERS = 3;
const NUM_CONSUMERS = 2;

const sharedBuffer = createSharedState(STORAGE_SIZE, TOTAL_ITEMS);

process.stdout.write(
    `Storage: ${STORAGE_SIZE} | Total items: ${TOTAL_ITEMS} | ` +
    `Producers: ${NUM_PRODUCERS} | Consumers: ${NUM_CONSUMERS}\n\n`
);

function spawnWorker(scriptPath: string, data: object): Promise<void> {
    return new Promise(resolve => {
        new Worker(scriptPath, { workerData: data }).on('exit', () => resolve());
    });
}

const workers: Promise<void>[] = [];

for (let i = 1; i <= NUM_PRODUCERS; i++) {
    const items = Math.floor(TOTAL_ITEMS / NUM_PRODUCERS) + (i <= TOTAL_ITEMS % NUM_PRODUCERS ? 1 : 0);
    workers.push(spawnWorker(path.join(__dirname, 'producer.js'), {
        producerId: i, itemsToProduce: items,
        totalItems: TOTAL_ITEMS, storageSize: STORAGE_SIZE, sharedBuffer
    }));
}

for (let i = 1; i <= NUM_CONSUMERS; i++) {
    const items = Math.floor(TOTAL_ITEMS / NUM_CONSUMERS) + (i <= TOTAL_ITEMS % NUM_CONSUMERS ? 1 : 0);
    workers.push(spawnWorker(path.join(__dirname, 'consumer.js'), {
        consumerId: i, itemsToConsume: items,
        storageSize: STORAGE_SIZE, sharedBuffer
    }));
}

Promise.all(workers).then(() => process.stdout.write('\nAll done.\n'));