import java.util.Arrays;
import java.util.List;
import java.util.Scanner;
import java.util.stream.Collectors;

public class App {
    private static final ArrayMinimumManager minManager = new ArrayMinimumManager();

    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);

        System.out.println("Enter array size");
        String sizeInput = scanner.nextLine();
        int size = sizeInput.trim().isEmpty() ? 10000000 : Integer.parseInt(sizeInput.trim());

        ArrayGenerator arrGen = new ArrayGenerator(size);

        System.out.println("Enter count of threads");
        String threadInput = scanner.nextLine();
        
        List<Integer> countOfThreads;
        if (threadInput.trim().isEmpty()) {
            countOfThreads = Arrays.asList(1, 2, 4, 8);
        } else {
            countOfThreads = Arrays.stream(threadInput.trim().split("\\s+"))
                                   .map(Integer::parseInt)
                                   .collect(Collectors.toList());
        }

        for (int count : countOfThreads) {
            System.out.println("\nTESTING WITH " + count + " THREADS");
            
            System.out.println("\n--Testing ArrayMinimumFinder");
            ArrayMinimumFinder threadFinder = new ArrayMinimumFinder(minManager, count);
            threadFinder.startFindingMin(arrGen.getGeneratedArray());
            System.out.println("----Found minimum: " + minManager.getMin());

            System.out.println("\n--Testing ArrayMinimumFinderWithCountDown");
            ArrayMinimumFinderWithCountDown taskFinder = new ArrayMinimumFinderWithCountDown(minManager, count);
            taskFinder.startFindingMin(arrGen.getGeneratedArray());
            System.out.println("----Found minimum: " + minManager.getMin());
        }

        scanner.close();
    }
}
