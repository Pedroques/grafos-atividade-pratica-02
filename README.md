# Algoritmos em Grafos - Atividade Prática 02

Este repositório contém o desenvolvimento da Atividade Prática 02 da disciplina de Algoritmos em Grafos. O projeto consiste em um sistema em C# voltado para a modelagem, análise e otimização logística de um Parque Turístico.

---

## 1. Descrição do Problema e Modelagem
O cenário proposto envolve a gestão de deslocamentos dentro de um parque ecológico composto por diferentes pontos de interesse (como mirantes, cachoeiras, áreas de descanso, restaurantes e centros de visitantes). 

O ambiente foi modelado matematicamente utilizando a teoria dos grafos, adotando-se as seguintes definições:
* **Vértices (V0 a V7):** Representam os 8 pontos de interesse físicos mapeados no parque.
* **Arestas:** Representam as trilhas físicas que interligam diretamente os pares de pontos.
* **Pesos:** Armazenam valores inteiros que correspondem à distância real em metros de cada trilha.
* **Propriedades do Grafo:** O modelo é definido como um grafo **não direcionado** (trilhas simétricas de sentido bidirecional), **ponderado** (presença de custos/distâncias) e **conexo** (garantia de que todos os pontos são acessíveis entre si).

---

## 2. Recursos Implementados

### Estruturas de Representação
* **Matriz de Adjacência (Implementado):** Matriz estática de dimensões 8x8 que armazena os pesos das trilhas. Valores iguais a `0` denotam a ausência de conexão direta. A estrutura conta com formatação de espaçamento fixo no console para facilitar a validação visual.
* **Lista de Adjacência:** *[Espaço reservado para integração da dupla]*

### Algoritmos de Menor Caminho
* **Algoritmo de Dijkstra via Matriz (Implementado):** Implementação gulosa que calcula os menores caminhos a partir de um vértice de origem customizado. O algoritmo exibe o custo mínimo acumulado em metros e reconstrói o caminho exato de forma recursiva através de um vetor de predecessores.
* **Algoritmo de Dijkstra via Lista:** *[Espaço reservado para integração da dupla]*

### Algoritmos de Árvore Geradora Mínima (AGM)
* **Algoritmo de Kruskal via Matriz (Implementado):** Algoritmo focado na otimização da infraestrutura de conectividade do parque. Realiza a ordenação crescente das arestas e utiliza a estrutura matemática *Union-Find* (com compressão de caminho) para evitar a formação de ciclos. Determina a configuração ótima de trilhas para manter todo o parque integrado com o menor somatório de distância possível.
* **Algoritmo de Prim via Lista:** *[Espaço reservado para integração da dupla]*

---

## 3. Execução do Projeto

### Pré-requisitos
* SDK do .NET (versão 6.0 ou superior) instalado no sistema operacional.

### Passo a Passo
1. Abra o terminal do seu sistema operacional na pasta raiz onde o projeto está localizado.
2. Execute o comando de compilação e inicialização:
   ```bash
   dotnet run
   ```

---

## 4. Cenário de Teste Padrão
Para otimizar o processo de validação acadêmica e evitar a inserção manual exaustiva de 64 campos na matriz a cada execução, o sistema conta com uma **função de carga automática (Opção 10)**. Esta opção pré-carrega o grafo conexo padrão utilizado para a geração dos relatórios.

Caso opte pela inserção customizada manual através da **Opção 1**, utilize a tabela de adjacência simétrica abaixo como guia para o preenchimento dos pesos linha por linha no console:

| Vértice | V0 | V1 | V2 | V3 | V4 | V5 | V6 | V7 |
|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|
| **V0** | 0 | 4 | 0 | 0 | 0 | 0 | 0 | 8 |
| **V1** | 4 | 0 | 8 | 0 | 0 | 0 | 0 | 11 |
| **V2** | 0 | 8 | 0 | 7 | 2 | 4 | 0 | 7 |
| **V3** | 0 | 0 | 7 | 0 | 9 | 14| 4 | 0 |
| **V4** | 0 | 0 | 2 | 9 | 0 | 10| 0 | 0 |
| **V5** | 0 | 0 | 4 | 14| 10| 0 | 2 | 0 |
| **V6** | 0 | 0 | 0 | 4 | 0 | 2 | 0 | 1 |
| **V7** | 8 | 11| 7 | 0 | 0 | 0 | 1 | 0 |
