using System;

namespace T_Generator
{
    public static class T_Holder
    {
        public const int MAX_SIZE = 100;
        public const int MIN_SIZE = 1;

        private static int startDist = 10;
        private static int distInterval = 3;

        private static float[] dist_Array = new float[MAX_SIZE];
        private static float[][] t_Array = new float[MAX_SIZE + 1][];

        static T_Holder()
        {
            Init_dist_Array();
            Init_t_Array();
        }
        // public methods
        public static float Get_t(int arrayLength, int index)
        {
            if (arrayLength < MIN_SIZE || arrayLength > MAX_SIZE) throw new ArgumentOutOfRangeException($"arrayLength is outOfRange[{MIN_SIZE}, {MAX_SIZE}]");
            if (index < 0 || index >= arrayLength) throw new ArgumentOutOfRangeException($"index is outOfRange[{0}, {arrayLength}]");
            return t_Array[arrayLength][index];
        }

        // private methods
        private static void Init_dist_Array()
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                dist_Array[i] = startDist + distInterval * i;
            }
        }
        private static void Init_t_Array()
        {
            t_Array[0] = new float[1] { 0f };
            t_Array[1] = new float[1] { 0.5f };
            t_Array[2] = new float[2] { 0, 1 };

            for (int i = 3; i <= MAX_SIZE; i++)
            {
                t_Array[i] = GenerateNumbers(i);
            }
        }
        private static float[] GenerateNumbers(int length)
        {
            float[] numbers = new float[length];
            bool isLengthEven = length % 2 == 0;

            if (isLengthEven) // for even length
            {
                int disLength = length / 2 - 1;

                float disToStart = 0;
                numbers[0] = 0;

                for (int i = 1; i <= disLength; i++)
                {
                    disToStart += dist_Array[i - 1];
                    numbers[i] = disToStart;
                }
                disToStart += dist_Array[disLength - 1];
                numbers[disLength + 1] = disToStart;

                int j = disLength - 1;
                for (int i = disLength + 2; i < length; i++)
                {
                    disToStart += dist_Array[j];
                    numbers[i] = disToStart;
                    j--;
                }

            }
            else // for odd length
            {
                int disLength = length / 2;

                float disToStart = 0;
                numbers[0] = 0;

                for (int i = 1; i <= disLength; i++)
                {
                    disToStart += dist_Array[i - 1];
                    numbers[i] = disToStart;
                }

                int j = disLength - 1;
                for (int i = disLength + 1; i < length; i++)
                {
                    disToStart += dist_Array[j];
                    numbers[i] = disToStart;
                    j--;
                }
            }

            // normalize:
            float max = numbers[length - 1];
            for (int i = 0; i < length; i++)
            {
                numbers[i] /= max;
            }

            return numbers;
        }
    }
}
