// Розмітка SharedArrayBuffer:
// [0] = access (mutex)
// [1] = full   (вільні місця)
// [2] = empty  (наявні предмети)
// [3] = writePtr (кільцевий буфер — куди писати)
// [4] = readPtr  (кільцевий буфер — звідки читати)
// [5] = totalProduced (глобальний лічильник)
// [6 .. 6+storageSize-1] = кільцевий буфер предметів

export const IDX = { ACCESS: 0, FULL: 1, EMPTY: 2, WRITE: 3, READ: 4, PRODUCED: 5, SIZE: 6, BUFFER: 7 };

export function createSharedState(storageSize: number, totalItems: number): SharedArrayBuffer {
    const buf = new SharedArrayBuffer((7 + storageSize) * Int32Array.BYTES_PER_ELEMENT);
    const arr = new Int32Array(buf);

    Atomics.store(arr, IDX.ACCESS, 1);
    Atomics.store(arr, IDX.FULL, storageSize);
    Atomics.store(arr, IDX.EMPTY, 0);
    Atomics.store(arr, IDX.WRITE, 0);
    Atomics.store(arr, IDX.READ, 0);
    Atomics.store(arr, IDX.PRODUCED, 0);
    Atomics.store(arr, IDX.SIZE, 0); // явний лічильник розміру

    return buf;
}