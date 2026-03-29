public class ArrayGenerator {
    private final int size;
    private boolean isGenerated = false;
    private int[] generatedArray;

    public ArrayGenerator(int size) {
        this.size = size;
        generatedArray = generate();
    }

    public int[] getGeneratedArray() {
        if (!isGenerated) {
            generatedArray = generate();
        }

        return generatedArray;
    }

    private int[] generate() {
        System.out.println("Generating array...");
        int[] arr = new int[size];
        for (int i = 0; i < size; i++) {
            arr[i] = i % 20000;
        }

        arr[arr.length - 1] = -1;
        isGenerated = true;
        return arr;
    }
}