namespace ServerCore
{
    // 힙트리
    // 1. [부모 노드]가 가진 값은 항상 [자식 노드]가 가진 값보다 크다.
    // 2. 마지막 층을 제외한 모든 층의 노드가 꽉 차 있다.
    // 3. 마지막 층에 노드가 채울 때는 항상 왼쪽부터 순서대로 채운다.

    // 다음과 같은 규칙 아래 배열로 힙 구조를 표현하기 쉬워짐
    // i 번 노드의 왼쪽 자식은 [(i * 2) + 1] 번
    // i 번 노드의 오른쪽 자식은 [(i * 2) + 2] 번
    // i 번 노드의 부모는 [(i - 1) / 2] 번

    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> heap = new List<T>();
        public int Count => heap.Count;

        // O(log N)
        public void Push(T data)
        {
            // 힙의 맨 끝에 새로운 데이터 삽입
            heap.Add(data);

            int now = heap.Count - 1;

            // 도장 깨기 시작
            while (now > 0)
            {
                int next = (now - 1) / 2;
                if (heap[now].CompareTo(heap[next]) < 0) // 내가 내 부모보다 작다면
                    break; // 도장깨기 실패

                // 도장깨기를 성공하면 두 값 교체
                T temp = heap[now];
                heap[now] = heap[next];
                heap[next] = temp;

                // 검사 위치 새로고침
                now = next;
            }
        }

        // O(log N)
        public T Pop()
        {
            // 반환할 데이터
            T result = heap[0];

            // 마지막 데이터를 루트로 이동
            int lastIndex = heap.Count - 1;
            heap[0] = heap[lastIndex];
            heap.RemoveAt(lastIndex);
            lastIndex--;

            // 역 도장깨기 시작
            int now = 0;
            while (true)
            {
                int left = 2 * now + 1;
                int right = 2 * now + 2;

                int next = now;
                // 왼쪽 값이 현재 값보다 크면 왼쪽으로 이동
                if (left <= lastIndex && heap[next].CompareTo(heap[left]) < 0)
                    next = left;
                // 오른쪽 값이 현재값(왼쪽 이동 포함)보다 크면, 오른쪽으로 이동
                if (right <= lastIndex && heap[next].CompareTo(heap[right]) < 0)
                    next = right;

                // 왼쪽 / 오른쪽 모두 현재값보다 작으면 종료
                if (next == now)
                    break;

                // 두 값 교체
                T temp = heap[now];
                heap[now] = heap[next];
                heap[next] = temp;

                // 검사 위치 이동
                now = next;
            }

            return result;
        }
    
        public T Peek()
        {
            if (heap.Count == 0)
                return default(T);
            return heap[0];
        }
    }
}
