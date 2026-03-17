import { ArrClass } from './ArrClass';
import * as os from 'os';

async function main(): Promise<void> {
    const cores = os.cpus().length;
    const DIM = 500_000_000;
    const threadCounts = [4, cores];

    console.log(`Доступних процесорів (ядер): ${cores}`);
    console.log(`Розмір масиву: ${DIM}`);
    console.log('='.repeat(60));

    for (const threadNum of threadCounts) {
        console.log(`\n>>> Запуск з кількістю потоків: ${threadNum}`);
        const arrClass = new ArrClass(DIM, threadNum);

        const startTime = Date.now();
        const [min, index] = await arrClass.threadMin();
        const elapsed = Date.now() - startTime;

        console.log(`Мінімальний елемент: ${min}, індекс: ${index}`);
        console.log(`Час виконання: ${elapsed} мс`);
        console.log('-'.repeat(60));
    }
}

main().catch(console.error);
