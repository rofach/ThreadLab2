public class ArrayMinimumFinder {
    private final ArrayMinimumManager manager;
    private final int threadCount;

    private final Object lockerForCount = new Object();
    private int completedThreadCount = 0;

    public ArrayMinimumFinder(ArrayMinimumManager manager, int threadCount) {
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
        completedThreadCount = 0;

        long startTime = System.currentTimeMillis();

        for (int i = 0; i < threadCount; i++) {
            Bound bound = getSegment(i, threadCount, size);
            new Thread(() -> threadWorker(arr, bound.start(), bound.end())).start();
        }

        synchronized (lockerForCount) {
            while (completedThreadCount < threadCount) {
                try {
                    lockerForCount.wait();
                } catch (InterruptedException e) {
                    Thread.currentThread().interrupt();
                }
            }
        }

        long endTime = System.currentTimeMillis();
        System.out.println("---Search time with Synchronized Threads: " + (endTime - startTime) + " ms");
    }

    private void threadWorker(int[] arr, int start, int end) {
        findMinInArr(arr, start, end);
        incThreadCount();
    }

    private void incThreadCount() {
        synchronized (lockerForCount) {
            completedThreadCount++;
            lockerForCount.notify();
        }
    }

    private static Bound getSegment(int index, int arrCount, int length) {
        int segmentLength = length / arrCount;
        int firstIndex = segmentLength * index;
        
        int lastIndex = (index == arrCount - 1) ? length : segmentLength * (index + 1);

        return new Bound(firstIndex, lastIndex);
    }
}