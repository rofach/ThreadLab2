import java.util.concurrent.locks.ReentrantLock;

public class ArrayMinimumManager {
    private int min = Integer.MAX_VALUE;
    private final ReentrantLock lock = new ReentrantLock();

    public void updateMin(int newMin) {
        lock.lock();
        try {
            if (newMin < min) {
                min = newMin;
            }
        } finally {
            lock.unlock();
        }
    }

    public int getMin() {
        lock.lock();
        try {
            return min;
        } finally {
            lock.unlock();
        }
    }
}