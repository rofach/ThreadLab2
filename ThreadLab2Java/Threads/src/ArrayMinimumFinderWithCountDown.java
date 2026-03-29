import java.util.concurrent.CountDownLatch;

public class ArrayMinimumFinderWithCountDown {
    private final ArrayMinimumManager manager;
    private final int threadCount;

    public ArrayMinimumFinderWithCountDown(ArrayMinimumManager manager, int threadCount) {
        this.manager = manager;
        this.threadCount = threadCount;
    }

    public void findMinInArr(int[] arr, int start, int end) {
        int min = arr[start];
        for (int i = start + 1; i < end; i++) {
            if (arr[i] < min) {
                min = arr[i];
            }
        }

        manager.updateMin(min);
    }

    public void startFindingMin(int[] arr) {
        int size = arr.length;

        long startTime = System.currentTimeMillis();

        CountDownLatch countdown = new CountDownLatch(threadCount);

        for (int i = 0; i < threadCount; i++) {
            Bound segment = getSegment(i, threadCount, size);
            new Thread(() -> threadWorker(arr, segment.start(), segment.end(), countdown)).start();
        }

        try {
            countdown.await();
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }

        long endTime = System.currentTimeMillis();
        System.out.println("---Search time with CountDownLatch: " + (endTime - startTime) + " ms");
    }

    private void threadWorker(int[] arr, int start, int end, CountDownLatch countdown) {
        try {
            findMinInArr(arr, start, end);
        } finally {
            countdown.countDown();
        }
    }

    private static Bound getSegment(int i, int arrCount, int length) {
        int segmentLength = length / arrCount;

        int firstIndex = segmentLength * i;
        int lastIndex = segmentLength * (i + 1);

        if (lastIndex > length) {
            lastIndex = length;
        }
    
        return new Bound(firstIndex, lastIndex);
    }
}