using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class Cache
    {
        // 캐시?
        // 메모리에서 데이터를 가져오는 건 많은 자원을 사용함
        // 따라서 한번 데이터를 가지러 갈 때 또는 데이터를 다시 반환할 때
        // 쓸 것 같은 데이터들은 캐시라는 공간에 미리 받아옴
        // 이 때 미리 쓸 것 같은 데이터를 판별하는 철학이 TEMPORAL, SPACIAL LOCALITY가 있음

        // TEMPORAL LOCALITY?
        // 최근에 사용한 데이터를 다시 사용할 확률이 높다는 철학
        // 따라서 데이터를 사용 후 바로 돌려보내는 것이 아닌 재사용을 위해 캐시에 보관해둠

        // SPACIAL LOCALITY
        // 메모리 구조적으로 보았을 때 방금 사용한 데이터 근처에 있는 데이터를 사용할 확률이 높다는 철학
        // 따라서 방금 사용한 데이터(주소) 근처에 있는 데이터를 캐시에 보관해둠

        public void Main()
        {
            // 5 * 5 배열이라 가정했을 때 메모리에 다음과 같이 공간이 확보될 것임
            // [][][][][] [][][][][] [][][][][] [][][][][] [][][][][]

            // 1번 케이스의 경우 순차적으로 값을 할당 (0, 1, 2, 3, 4, 5, ...)
            // 이와 같은 경우 SPACIAL LOCALITY에 의해서 주변 값들이 미리 캐싱되어 있기에
            // 메모리에서 값을 받아오는 시간을 최소화 시킬 수 있음

            // 2번 케이스의 경우 뒤죽박줄 값을 할당 (0, 5, 10, 15, 20, 1, 6, 11, ...)
            // 이와 같은 경우 1번 케이스와 달리 뒤죽박죽 접근하기 때문에
            // 1번 케이스에 비해 많은 시간을 소모하게 됨

            // 1번 케이스 실행 시간 3320129
            // 2번 케이스 실행 시간 5964038

            int[,] arr = new int[10000, 10000];


            // 케이스 1
            {
                long now = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; y++)
                    for (int x = 0; x < 10000; x++)
                        arr[y, x] = 1;

                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(y, x) 순서 걸린 시간 {end - now}");
            }

            // 케이스 2
            {
                long now = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; y++)
                    for (int x = 0; x < 10000; x++)
                        arr[x, y] = 1;
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(x, y) 순서 걸린 시간 {end - now}");
            }
        }
    }
}
