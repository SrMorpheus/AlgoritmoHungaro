using AlgoritmoHungaro;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;

int[,] cost1 = new int[31, 31];
int n, maxMatch;

int[] lx = new int[31];
int[] ly = new int[31];

int [] xy = new int[31];
int[] yx = new int[31];

bool[]S = new bool[31];
bool[]T = new bool[31];

int[] slack = new int[31];
int[] slackx = new int[31];

int []previous = new int[31];


 static void Memset<T>(T[] array, T elem)
{
    int length = array.Length;
    if (length == 0) return;
    array[0] = elem;
    int count;
    for (count = 1; count <= length / 2; count *= 2)
        Array.Copy(array, 0, array, count, count);
    Array.Copy(array, 0, array, count, length - count);
}




 void InitLabel()
{

    Memset(lx, 0);
    Memset(ly, 0);

    for (int x = 0; x < n; x++)
    for (int y = 0; y < n; y++)
    lx[x] = Math.Max(lx[x], cost1[x, y]);

}




void UpdateLabels()
{

    int x, y;

    int delta = 99999999;

    for (y = 0; y < n; y++)
        if (!T[y])
            delta = Math.Min(delta, slack[y]);
    for(x = 0; x < n; x++)
        if (S[x])
            lx[x] -= delta;
    for (y = 0; y < n; y++)
        if (T[y])
            ly[y] += delta;
    for (y = 0; y < n; y++)
        if (!T[y])
            slack[y] -= delta;

}



void AddToTree( int x , int PrevIousx)
{

    S[x] = true;

    previous[x] = PrevIousx;

    for(int y = 0; y< n; y++)
    {
        if (lx[x] + ly[y] - cost1[x,y] < slack[y])
        {
            slack[y] = lx[x] + ly[y] - cost1[x,y];
            slackx[y] = x;

        }
    }

}


void Augment()
{
    if (maxMatch == n) return;
    int x, y, root = 0;
    int[] q = new int[31];
    int wr = 0, rd = 0;

    Memset(S, false);
    Memset(T, false);
    Memset(previous, -1);

    for(x = 0; x < n; x++)
    {
        if (xy[x] == -1)
        {
            q[wr++] = root = x;
            previous[x] = -2;
            S[x] = true;
            break;
        }

    }
    for(y = 0; y < n; y++)
    {
        slack[y] = lx[root] + ly[y] - cost1[root, y];
        slackx[y] = root;


    }

    while(true)
    {
        while(rd < wr)
        {
            x = q[rd++];
            for (y = 0; y < n; y++)
                if (cost1[x,y] == lx[x] + ly[y] && !T[y])
                {
                    if (yx[y] == -1) break;
                        T[y] = true;
                    q[wr++] = yx[y];
                    AddToTree(yx[y], x);
                }
            if (y < n)
                break;

        }
        if (y < n)
            break;

        UpdateLabels();

        wr = rd = 0;
        for(y = 0; y < n; y ++)
        if (!T[y] && slack[y] == 0)
         {
                if (yx[y] == -1)
                {
                    x = slackx[y];
                    break;

                }else
                {
                    T[y] = true;
                    if (!S[yx[y]])
                    {

                        q[wr++] = yx[y];
                        AddToTree(yx[y], slackx[y]);
                    }

                }

         }

        if (y < n) break;


    }
    if (y < n)
    {

        maxMatch++;

        for (int cx = x, cy = y, ty = 0; cx != -2; cx = previous[cx], cy = ty)
        {

            ty = xy[cx];
            yx[cy] = cx;
            xy[cx] = cy;

        }


        Augment();

    }


}

 List<int> Hungarian()
{

    List<int> caminhosInstancia  = new List<int>();
    int ret = 0;
    maxMatch = 0;
    Memset(xy, -1);
    Memset(yx, -1);

    InitLabel();

    Augment();

    for (int x = 0; x < n; x ++)
    {


        caminhosInstancia.Add(cost1[x, xy[x]] * -1);

    }


    

    return caminhosInstancia;
    

}


List<int> assignmentProblem(int []Arr, int N)
{

    n = N;
    for (int i = 0; i < n; i++)
        for (int j = 0; j < n; j++)
            cost1[i,j] = -1 * Arr[i * n + j];

    List<int> ans = Hungarian();



    return ans;
}



int n1 = 3;
int soma = 0;
int[] Arr = new int[9] { 2500, 4000, 3500, 4000, 6000, 3500, 2000, 4000, 2500 };
List<int> rotas = new List<int>();


rotas = assignmentProblem(Arr, n1);


Console.WriteLine("A rota ideal essa:");
foreach(var rota in rotas)
{
    soma += rota;
    Console.WriteLine(rota);

}


Console.WriteLine("A soma do custo ideal é: " + soma);







