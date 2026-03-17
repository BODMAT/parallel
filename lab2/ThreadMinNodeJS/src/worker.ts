import { workerData, threadId } from 'worker_threads';

const { threadId: tid, startIndex, finishIndex, sharedBuffer, syncBuffer } = workerData as {
    threadId: number;
    startIndex: number;
    finishIndex: number;
    sharedBuffer: SharedArrayBuffer; // масив даних
    syncBuffer: SharedArrayBuffer;   // синхронізаційні змінні
};

// Індекси в syncBuffer (аналог полів класу в Java)
const LOCK = 0; // спін-лок для collectMin
const GLOBAL_MIN = 1; // globalMin
const GLOBAL_IDX = 2; // globalMinIndex
const COUNT = 3; // threadCount

const arr = new Int32Array(sharedBuffer);
const syncArr = new Int32Array(syncBuffer);

console.log(`Потік #${tid} | Ім'я: Worker-${threadId} | Діапазон: [${startIndex}, ${finishIndex})`);

// пошук локального мінімуму (без синхронізації — тільки читання)
let localMin = arr[startIndex];
let localMinIndex = startIndex;
let acc = 0.0;

for (let i = startIndex + 1; i < finishIndex; i++) {
    //! Симуляція для наглядності
    const unused = Math.sqrt(Math.abs(arr[i])) * Math.sin(arr[i]);
    acc += unused;
    if (arr[i] < localMin) {
        localMin = arr[i];
        localMinIndex = i;
    }
}

console.log(`Потік #${tid} | Локальний мін: ${localMin} за індексом: ${localMinIndex}`);

//! Критична секція через спін-лок (аналог synchronized) ---
// Atomics.compareExchange(arr, idx, expected, replacement)
// Якщо arr[LOCK] === 0 (вільний) - замінює на 1 (зайнятий) і повертає 0
// Якщо arr[LOCK] === 1 (зайнятий) - нічого не міняє і повертає 1 - крутимось далі
while (Atomics.compareExchange(syncArr, LOCK, 0, 1) !== 0) {
    // spin
}

// Всередині критичної секції — тільки один потік тут одночасно
if (localMin < Atomics.load(syncArr, GLOBAL_MIN)) {
    Atomics.store(syncArr, GLOBAL_MIN, localMin);
    Atomics.store(syncArr, GLOBAL_IDX, localMinIndex);
}

Atomics.store(syncArr, LOCK, 0); // Звільняю лок, а отже хтось якщо є - зайде в новий лок

Atomics.add(syncArr, COUNT, 1); // аналог threadCount++;
Atomics.notify(syncArr, COUNT, 1); // будимо головний потік
