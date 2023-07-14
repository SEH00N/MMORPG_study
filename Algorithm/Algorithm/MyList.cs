namespace Algorithm
{
    public class MyList<T>
    {
        const int DEFAULT_Size = 1;
        private T[] data = new T[DEFAULT_Size];

        public int Count = 0;
        public int Capacity => data.Length;

        // O(1)
        public T this[int index] {
            get => data[index];
            set {
                data[index] = value;
            }
        }

        // O(N) 아님? 이라고 생각할 수 있지만
        // for문이 돌아가는 경우는 특수한 경우이기 때문에
        // O(1) 이라고 표기
        public void Add(T item)
        {
            // 공간이 없으면
            if (Count >= Capacity) 
            {
                // 공간을 다시 확보
                T[] newData = new T[Count * 2];
                for (int i = 0; i < Count; i++)
                    newData[i] = data[i];

                data = newData;
            }

            // 데이터 삽입
            data[Count++] = item;
        }

        // 최고의 경우 1번만 반복하는 경우도 생기지만
        // Big-O 표기법으로 표기할 땐 최악의 경우로 생각하기 때문에
        // O(N) 이라고 표기
        public void RemoveAt(int index)
        {
            for(int i = index; i < Count - 1; i++)
                data[i] = data[i + 1];

            data[Count - 1] = default(T);
            Count--;
        }
    }
}
