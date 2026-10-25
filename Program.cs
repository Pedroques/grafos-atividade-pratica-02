using System;
using System.Collections.Generic;

class Program
{
    // =========================================================================
    // 01) INICIALIZAÇÃO DA MATRIZ E VARIÁVEIS GLOBAIS (PEDRO MARTINS)
    // =========================================================================
    // Matriz 8x8 que agora armazena os pesos (distâncias). O valor 0 indica ausência de trilha.
    static int[,] matrizAdjacencia = new int[8, 8];
    static int verticeOrigem = 0;
    static bool grafoConfigurado = false;

    // Lista de adjacência usada na versão da Ana.
    static List<ArestaLista>[] listaAdjacencia = CriarListaAdjacencia();
    static bool listaConfigurada = false;

    class ArestaLista
    {
        public int Destino { get; set; }
        public int Distancia { get; set; }

        public ArestaLista(int destino, int distancia)
        {
            Destino = destino;
            Distancia = distancia;
        }
    }

    static List<ArestaLista>[] CriarListaAdjacencia()
    {
        List<ArestaLista>[] lista = new List<ArestaLista>[8];
        for (int i = 0; i < 8; i++)
            lista[i] = new List<ArestaLista>();
        return lista;
    }

    static void AdicionarTrilhaLista(int u, int v, int distancia)
    {
        listaAdjacencia[u].Add(new ArestaLista(v, distancia));
        listaAdjacencia[v].Add(new ArestaLista(u, distancia));
    }

    // Estrutura interna no seu estilo para organizar as arestas no Kruskal
    struct Trilha : IComparable<Trilha>
    {
        public int Origem;
        public int Destino;
        public int Distancia;

        public Trilha(int origem, int destino, int distancia)
        {
            Origem = origem;
            Destino = destino;
            Distancia = distancia;
        }

        public int CompareTo(Trilha outra)
        {
            return this.Distancia.CompareTo(outra.Distancia);
        }
    }

    // =========================================================================
    // 02) FUNÇÕES PARA CONFIGURAÇÃO DO GRAFO (SUA PARTE)
    // =========================================================================

    // Alimenta o cenário do parque automaticamente com dados conexos e válidos para testes rápidos
    static void InicializarGrafoPadrao()
    {
        Array.Clear(matrizAdjacencia, 0, matrizAdjacencia.Length);

        // Configuração das trilhas e distâncias reais entre os pontos V0 a V7
        ConfigurarTrilhaMatriz(0, 1, 4);
        ConfigurarTrilhaMatriz(0, 7, 8);
        ConfigurarTrilhaMatriz(1, 2, 8);
        ConfigurarTrilhaMatriz(1, 7, 11);
        ConfigurarTrilhaMatriz(2, 3, 7);
        ConfigurarTrilhaMatriz(2, 4, 2);
        ConfigurarTrilhaMatriz(2, 5, 4);
        ConfigurarTrilhaMatriz(2, 7, 7);
        ConfigurarTrilhaMatriz(3, 4, 9);
        ConfigurarTrilhaMatriz(3, 5, 14);
        ConfigurarTrilhaMatriz(3, 6, 4);
        ConfigurarTrilhaMatriz(4, 5, 10);
        ConfigurarTrilhaMatriz(5, 6, 2);
        ConfigurarTrilhaMatriz(6, 7, 1);

        // Carrega o mesmo cenário padrão na lista de adjacência.
        listaAdjacencia = CriarListaAdjacencia();
        AdicionarTrilhaLista(0, 1, 4);
        AdicionarTrilhaLista(0, 7, 8);
        AdicionarTrilhaLista(1, 2, 8);
        AdicionarTrilhaLista(1, 7, 11);
        AdicionarTrilhaLista(2, 3, 7);
        AdicionarTrilhaLista(2, 4, 2);
        AdicionarTrilhaLista(2, 5, 4);
        AdicionarTrilhaLista(2, 7, 7);
        AdicionarTrilhaLista(3, 4, 9);
        AdicionarTrilhaLista(3, 5, 14);
        AdicionarTrilhaLista(3, 6, 4);
        AdicionarTrilhaLista(4, 5, 10);
        AdicionarTrilhaLista(5, 6, 2);
        AdicionarTrilhaLista(6, 7, 1);

        grafoConfigurado = true;
        listaConfigurada = true;
    }

    static void ConfigurarTrilhaMatriz(int u, int v, int distancia)
    {
        matrizAdjacencia[u, v] = distancia;
        matrizAdjacencia[v, u] = distancia; // Grafo não direcionado (Simetria)
    }

    // Permite que o usuário defina manualmente as distâncias de cada trilha do parque
    static void DefinirVerticesEArestasMatriz()
    {
        Console.Clear();
        Console.WriteLine("=== Definindo Distâncias das Trilhas por Matriz (0 se não houver) ===");

        for (int i = 0; i < 8; i++)
        {
            for (int j = i; j < 8; j++) // Percorre apenas a metade superior para poupar digitação simétrica
            {
                if (i == j)
                {
                    matrizAdjacencia[i, j] = 0;
                    continue;
                }

                Console.Write($"Digite a distância da trilha entre o vértice {i} e o vértice {j}: ");
                int distancia = int.Parse(Console.ReadLine());
                ConfigurarTrilhaMatriz(i, j, distancia);
            }
            Console.WriteLine();
        }

        grafoConfigurado = true;
        Console.WriteLine("\nGrafo via Matriz configurado com sucesso! Pressione ENTER para continuar...");
        Console.ReadKey();
        Console.Clear();
    }

    // Define o ponto do parque de onde partirão os cálculos do Dijkstra
    static void DefinirVerticeOrigem()
    {
        Console.Clear();
        Console.Write("Digite o vértice de origem para o Dijkstra (0 a 7): ");
        int entrada = int.Parse(Console.ReadLine());

        if (entrada >= 0 && entrada < 8)
        {
            verticeOrigem = entrada;
            Console.WriteLine($"Vértice de origem fixado em: V{verticeOrigem}.");
        }
        else
        {
            Console.WriteLine("Vértice inválido! Operação cancelada.");
        }

        Console.WriteLine("Pressione ENTER para continuar...");
        Console.ReadKey();
        Console.Clear();
    }

    // =========================================================================
    // FUNÇÕES DA LISTA DE ADJACÊNCIA (ANA)
    // =========================================================================

    // Permite cadastrar as trilhas para a versão que usa lista de adjacência.
    // Informe 0 quando não existir trilha direta entre os vértices.
    static void DefinirVerticesEArestasLista()
    {
        Console.Clear();
        listaAdjacencia = CriarListaAdjacencia();

        Console.WriteLine("=== Configuração por Lista de Adjacência ===");
        Console.WriteLine("Informe a distância em metros; digite 0 se não houver trilha.");
        Console.WriteLine("O programa pergunta cada par de vértices apenas uma vez.\n");

        for (int i = 0; i < 8; i++)
        {
            for (int j = i + 1; j < 8; j++)
            {
                int distancia;
                while (true)
                {
                    Console.Write($"Distância entre V{i} e V{j} (0 = sem trilha): ");
                    if (int.TryParse(Console.ReadLine(), out distancia) && distancia >= 0)
                        break;

                    Console.WriteLine("Digite um número inteiro maior ou igual a zero.");
                }

                if (distancia > 0)
                    AdicionarTrilhaLista(i, j, distancia);
            }
        }

        listaConfigurada = true;
        Console.WriteLine("\nLista de adjacência configurada. Pressione ENTER para voltar...");
        Console.ReadLine();
        Console.Clear();
    }

    static void ImprimirListaAdjacencia()
    {
        Console.Clear();
        if (!listaConfigurada)
            InicializarGrafoPadrao();

        Console.WriteLine("=== Lista de Adjacência (trilhas e distâncias) ===\n");
        for (int i = 0; i < 8; i++)
        {
            Console.Write($"V{i}: ");
            if (listaAdjacencia[i].Count == 0)
            {
                Console.WriteLine("sem trilhas conectadas");
                continue;
            }

            for (int j = 0; j < listaAdjacencia[i].Count; j++)
            {
                ArestaLista aresta = listaAdjacencia[i][j];
                Console.Write($"V{aresta.Destino} ({aresta.Distancia} m)");
                if (j < listaAdjacencia[i].Count - 1)
                    Console.Write(" -> ");
            }
            Console.WriteLine();
        }

        Console.WriteLine("\nPressione ENTER para voltar ao menu...");
        Console.ReadLine();
        Console.Clear();
    }

    static void ExecutarDijkstraLista()
    {
        Console.Clear();
        if (!listaConfigurada)
            InicializarGrafoPadrao();

        Console.WriteLine($"=== Dijkstra via Lista de Adjacência (origem: V{verticeOrigem}) ===\n");

        int[] distancias = new int[8];
        int[] predecessores = new int[8];
        bool[] visitados = new bool[8];

        for (int i = 0; i < 8; i++)
        {
            distancias[i] = int.MaxValue;
            predecessores[i] = -1;
        }
        distancias[verticeOrigem] = 0;

        for (int passo = 0; passo < 8; passo++)
        {
            int atual = -1;
            int menor = int.MaxValue;

            for (int v = 0; v < 8; v++)
            {
                if (!visitados[v] && distancias[v] < menor)
                {
                    menor = distancias[v];
                    atual = v;
                }
            }

            if (atual == -1)
                break;

            visitados[atual] = true;

            foreach (ArestaLista aresta in listaAdjacencia[atual])
            {
                int vizinho = aresta.Destino;
                if (!visitados[vizinho] &&
                    distancias[atual] != int.MaxValue &&
                    distancias[atual] + aresta.Distancia < distancias[vizinho])
                {
                    distancias[vizinho] = distancias[atual] + aresta.Distancia;
                    predecessores[vizinho] = atual;
                }
            }
        }

        for (int v = 0; v < 8; v++)
        {
            Console.Write($"Destino V{v}: ");
            if (distancias[v] == int.MaxValue)
            {
                Console.WriteLine("inalcançável");
                continue;
            }

            Console.WriteLine($"menor distância = {distancias[v]} m");
            Console.Write("Caminho: ");
            RastrearCaminhoDijkstra(predecessores, v);
            Console.WriteLine("\n");
        }

        Console.WriteLine("Pressione ENTER para voltar...");
        Console.ReadLine();
        Console.Clear();
    }

    static void ExecutarPrimLista()
    {
        Console.Clear();
        if (!listaConfigurada)
            InicializarGrafoPadrao();

        Console.WriteLine("=== Prim via Lista de Adjacência (Árvore Geradora Mínima) ===\n");

        bool[] incluidos = new bool[8];
        int[] menoresPesos = new int[8];
        int[] pais = new int[8];

        for (int i = 0; i < 8; i++)
        {
            menoresPesos[i] = int.MaxValue;
            pais[i] = -1;
        }
        menoresPesos[0] = 0;

        int custoTotal = 0;
        int verticesProcessados = 0;

        for (int passo = 0; passo < 8; passo++)
        {
            int atual = -1;
            int menor = int.MaxValue;

            for (int v = 0; v < 8; v++)
            {
                if (!incluidos[v] && menoresPesos[v] < menor)
                {
                    menor = menoresPesos[v];
                    atual = v;
                }
            }

            if (atual == -1)
                break;

            incluidos[atual] = true;
            verticesProcessados++;

            foreach (ArestaLista aresta in listaAdjacencia[atual])
            {
                int vizinho = aresta.Destino;
                if (!incluidos[vizinho] && aresta.Distancia < menoresPesos[vizinho])
                {
                    menoresPesos[vizinho] = aresta.Distancia;
                    pais[vizinho] = atual;
                }
            }
        }

        if (verticesProcessados != 8)
        {
            Console.WriteLine("Não foi possível gerar a árvore: o grafo não é conexo.");
        }
        else
        {
            Console.WriteLine("Trilhas selecionadas:");
            for (int v = 1; v < 8; v++)
            {
                Console.WriteLine($"V{pais[v]} <-> V{v} | Distância: {menoresPesos[v]} m");
                custoTotal += menoresPesos[v];
            }

            Console.WriteLine($"\nDistância total da árvore: {custoTotal} m");
            Console.WriteLine("Quantidade de trilhas selecionadas: 7");
        }

        Console.WriteLine("\nPressione ENTER para voltar...");
        Console.ReadLine();
        Console.Clear();
    }

    // =========================================================================
    // 03) FUNÇÕES DE IMPRESSÃO E VISUALIZAÇÃO (PEDRO MARTINS)
    // =========================================================================
    static void ImprimirMatrizAdjacencia()
    {
        Console.Clear();
        Console.WriteLine("=== Matriz de Adjacência (Pesos/Distâncias das Trilhas) ===");

        Console.Write("V#  |");
        for (int j = 0; j < 8; j++)
        {
            Console.Write($" V{j}  ");
        }
        Console.WriteLine("\n-----------------------------------------------------------");

        for (int i = 0; i < 8; i++)
        {
            Console.Write($"V{i}  |");

            for (int j = 0; j < 8; j++)
            {
                Console.Write($"{matrizAdjacencia[i, j],3} ");
            }
            Console.WriteLine();
        }

        Console.WriteLine("\n*Valores > 0 indicam a distância real em metros.");
        Console.WriteLine("Pressione ENTER para voltar ao menu...");
        Console.ReadKey();
        Console.Clear();
    }

    // =========================================================================
    // 04) IMPLEMENTAÇÃO DOS ALGORITMOS EM GRAFOS (PEDRO MARTINS)
    // =========================================================================

    // Algoritmo de Dijkstra focado em Matriz de Adjacência
    static void ExecutarDijkstraMatriz()
    {
        Console.Clear();
        if (!grafoConfigurado) InicializarGrafoPadrao();

        Console.WriteLine($"=== Algoritmo de Dijkstra via Matriz (Origem: V{verticeOrigem}) ===");
        Console.WriteLine("Pergunta: 'Qual é a menor distância para chegar aos outros pontos do parque?'\n");

        int[] distancias = new int[8];
        bool[] visitados = new bool[8];
        int[] predecessores = new int[8];

        for (int i = 0; i < 8; i++)
        {
            distancias[i] = int.MaxValue;
            visitados[i] = false;
            predecessores[i] = -1;
        }

        distancias[verticeOrigem] = 0;

        for (int c = 0; c < 7; c++)
        {
            int u = ObterMinimaDistancia(distancias, visitados);
            if (u == -1) break;

            visitados[u] = true;

            for (int v = 0; v < 8; v++)
            {
                if (!visitados[v] && matrizAdjacencia[u, v] != 0 &&
                    distancias[u] != int.MaxValue &&
                    distancias[u] + matrizAdjacencia[u, v] < distancias[v])
                {
                    distancias[v] = distancias[u] + matrizAdjacencia[u, v];
                    predecessores[v] = u;
                }
            }
        }

        // Apresentação dos caminhos e custos finais na tela
        for (int i = 0; i < 8; i++)
        {
            Console.Write($"Para V{i}: Menor Custo = ");
            if (distancias[i] == int.MaxValue)
            {
                Console.WriteLine("Inacessível");
            }
            else
            {
                Console.Write($"{distancias[i],2}m | Rota: ");
                RastrearCaminhoDijkstra(predecessores, i);
                Console.WriteLine();
            }
        }

        Console.WriteLine("\nPressione ENTER para retornar...");
        Console.ReadKey();
        Console.Clear();
    }

    static int ObterMinimaDistancia(int[] distancias, bool[] visitados)
    {
        int min = int.MaxValue, minIndex = -1;

        for (int v = 0; v < 8; v++)
        {
            if (!visitados[v] && distancias[v] <= min)
            {
                min = distancias[v];
                minIndex = v;
            }
        }
        return minIndex;
    }

    static void RastrearCaminhoDijkstra(int[] predecessores, int v)
    {
        if (predecessores[v] == -1)
        {
            Console.Write($"V{v}");
            return;
        }
        RastrearCaminhoDijkstra(predecessores, predecessores[v]);
        Console.Write($" -> V{v}");
    }

    // Algoritmo de Kruskal (Gulozo + Estrutura Union-Find para controle de ciclos)
    static void ExecutarKruskalMatriz()
    {
        Console.Clear();
        if (!grafoConfigurado) InicializarGrafoPadrao();

        Console.WriteLine("=== Algoritmo de Kruskal via Matriz (Árvore Geradora Mínima) ===");
        Console.WriteLine("Pergunta: 'Quais trilhas seriam suficientes para manter todos os pontos interligados com a menor distância total?'\n");

        List<Trilha> listaTrilhas = new List<Trilha>();

        // Captura as trilhas contidas na matriz sem duplicar (i < j)
        for (int i = 0; i < 8; i++)
        {
            for (int j = i + 1; j < 8; j++)
            {
                if (matrizAdjacencia[i, j] != 0)
                {
                    listaTrilhas.Add(new Trilha(i, j, matrizAdjacencia[i, j]));
                }
            }
        }

        // Ordena de forma crescente para a escolha gulosa
        listaTrilhas.Sort();

        int[] subconjuntos = new int[8];
        for (int i = 0; i < 8; i++) subconjuntos[i] = i;

        int custoTotalAGM = 0;
        List<Trilha> resultadoAGM = new List<Trilha>();

        foreach (var trilha in listaTrilhas)
        {
            int raizOrigem = EncontrarConjunto(subconjuntos, trilha.Origem);
            int raizDestino = EncontrarConjunto(subconjuntos, trilha.Destino);

            if (raizOrigem != raizDestino)
            {
                resultadoAGM.Add(trilha);
                custoTotalAGM += trilha.Distancia;
                UnificarConjuntos(subconjuntos, raizOrigem, raizDestino);
            }
        }

        Console.WriteLine("Trilhas selecionadas para a infraestrutura do parque:");
        foreach (var trilha in resultadoAGM)
        {
            Console.WriteLine($"  Trilha: V{trilha.Origem} -- V{trilha.Destino} (Distância: {trilha.Distancia}m)");
        }

        Console.WriteLine($"\n[+] DISTÂNCIA TOTAL REQUERIDA (AGM): {custoTotalAGM} metros.");
        Console.WriteLine("\nPressione ENTER para retornar...");
        Console.ReadKey();
        Console.Clear();
    }

    static int EncontrarConjunto(int[] pai, int i)
    {
        if (pai[i] == i)
            return i;
        return pai[i] = EncontrarConjunto(pai, pai[i]); // Compressão de caminho
    }

    static void UnificarConjuntos(int[] pai, int raizX, int raizY)
    {
        pai[raizX] = raizY;
    }

    // =========================================================================
    // 05) MENU PRINCIPAL E ESQUELETO DE INTEGRAÇÃO (DA DUPLA)
    // =========================================================================
    static void Main()
    {
        InicializarGrafoPadrao();
        int opcaoEscolhida;

        do
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("   SISTEMA DE GESTÃO - PARQUE TURÍSTICO (GRAFOS)  ");
            Console.WriteLine("==================================================");
            Console.WriteLine("1. Definir Vértices e Trilhas por Matriz");
            Console.WriteLine("2. Definir por Lista de Adjacência");
            Console.WriteLine("3. Definir Vértice de Origem Geral");
            Console.WriteLine("4. Imprimir Matriz de Adjacência");
            Console.WriteLine("5. Imprimir Lista de Adjacência");
            Console.WriteLine("6. Executar Dijkstra via Matriz");
            Console.WriteLine("7. Executar Dijkstra via Lista");
            Console.WriteLine("8. Executar Kruskal via Matriz (AGM)");
            Console.WriteLine("9. Executar Prim via Lista (AGM)");
            Console.WriteLine("10. Restaurar Cenário de Teste Padrão (8 Vértices)");
            Console.WriteLine("11. Sair.");
            Console.WriteLine("--------------------------------------------------");
            Console.Write("Digite o número da opção desejada: ");

            if (!int.TryParse(Console.ReadLine(), out opcaoEscolhida))
            {
                opcaoEscolhida = 0;
            }

            switch (opcaoEscolhida)
            {
                case 1:
                    DefinirVerticesEArestasMatriz();
                    break;
                case 2:
                    DefinirVerticesEArestasLista();
                    break;
                case 3:
                    DefinirVerticeOrigem();
                    break;
                case 4:
                    ImprimirMatrizAdjacencia();
                    break;
                case 5:
                    ImprimirListaAdjacencia();
                    break;
                case 6:
                    ExecutarDijkstraMatriz();
                    break;
                case 7:
                    ExecutarDijkstraLista();
                    break;
                case 8:
                    ExecutarKruskalMatriz();
                    break;
                case 9:
                    ExecutarPrimLista();
                    break;
                case 10:
                    InicializarGrafoPadrao();
                    Console.Clear();
                    Console.WriteLine("Grafo padrão reestabelecido com sucesso!\n");
                    break;
                case 11:
                    Console.WriteLine("\nSaindo... Obrigado por utilizar o sistema!");
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Opção inválida! Escolha de 1 a 11.\n");
                    break;
            }
        } while (opcaoEscolhida != 11);
    }
}