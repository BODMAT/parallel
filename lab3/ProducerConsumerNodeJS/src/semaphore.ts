export class Semaphore {
    private readonly syncArr: Int32Array;
    private readonly idx: number;

    // initialValue = -1 означає "не ініціалізувати" (вже задано в Manager)
    constructor(syncArr: Int32Array, idx: number, initialValue: number = -1) {
        this.syncArr = syncArr;
        this.idx = idx;
        if (initialValue >= 0) {
            Atomics.store(this.syncArr, this.idx, initialValue);
        }
    }

    acquire(): void {
        while (true) {
            let val = Atomics.load(this.syncArr, this.idx); // читаю поточне значення лічильника з розділеної пам'яті (атомарно)
            while (val > 0) {
                // якщо зараз val — постав val-1, інакше нічого не роби
                if (Atomics.compareExchange(this.syncArr, this.idx, val, val - 1) === val) {
                    return;
                }
                val = Atomics.load(this.syncArr, this.idx); // перечитує актуальне значення
            }
            Atomics.wait(this.syncArr, this.idx, 0);
        }
    }

    release(): void {
        Atomics.add(this.syncArr, this.idx, 1); // атомарно +1
        Atomics.notify(this.syncArr, this.idx, 1); // будим 1 поток
    }
}