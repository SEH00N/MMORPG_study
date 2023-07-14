namespace Algorithm
{
    public class MyLinkedListNode<T>
    {
        public T Data = default(T);
        public MyLinkedListNode<T> Next = null;
        public MyLinkedListNode<T> Prev = null;
    }

    public class MyLinkedList<T>
    {
        public MyLinkedListNode<T> Left = null;  // 첫번째 값
        public MyLinkedListNode<T> Right = null; // 마지막 값
        public int Count = 0;

        // O(1)
        public MyLinkedListNode<T> AddLast(T data)
        {
            MyLinkedListNode<T> newNode = new MyLinkedListNode<T>();
            newNode.Data = data;
            
            // 비어있는 리스트라면 첫번째 노드 할당
            if (Left == null)
                Left = newNode;

            // 마지막 노드가 비어있지 않다면 새 노드와 연결
            if (Right != null)
            {
                Right.Next = newNode;
                newNode.Prev = Right;
            }

            // 마지막 노드 갱신
            Right = newNode;
            Count++;

            return newNode;
        }

        // O(1)
        public void Remove(MyLinkedListNode<T> node)
        {
            // 첫번째 노드를 지운다면 첫번째 노드를 다음 노드로 갱신
            if (Left == node)
                Left = Left.Next;

            // 마지막 노드를 지운다면 마지막 노드를 전 노드로 갱신
            if (Right == node)
                Right = Right.Prev;

            if (node.Prev != null)
                node.Prev.Next = node.Next;

            if (node.Next != null)
                node.Next.Prev = node.Prev;

            Count--;
        }
    }
}
