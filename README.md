# PetDelivery API

![.NET](https://img.shields.io/badge/.NET-8-blueviolet)
![C#](https://img.shields.io/badge/C%23-11-green)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-14.5-blue)
![Arquitetura](https://img.shields.io/badge/Arquitetura-Clean-orange)

API robusta para uma plataforma de e-commerce e marketplace focada no mercado de produtos para pets. Desenvolvido como um projeto de portfólio para demonstrar habilidades avançadas em desenvolvimento back-end com .NET, aplicando princípios de **Clean Architecture** e design orientado a domínio (DDD-Lite).

## 📄 Sobre o Projeto

O PetDelivery simula o back-end de um sistema completo de marketplace, onde múltiplos vendedores podem se cadastrar, gerenciar seus produtos, e clientes podem navegar, comprar e gerenciar seus pedidos. A API foi projetada com foco em escalabilidade, manutenibilidade e separação de responsabilidades.

## ✨ Principais Funcionalidades

-   **👤 Gestão de Usuários e Autenticação:**
    -   Cadastro de clientes e vendedores.
    -   Autenticação segura via **Tokens JWT** com senhas criptografadas (BCrypt).
    -   Gerenciamento de perfil e múltiplos endereços por usuário.

-   **🛍️ Jornada de Compra Completa:**
    -   Catálogo de produtos com buscas, filtros por categoria e ordenação.
    -   Carrinho de compras persistente por usuário.
    -   Fluxo de checkout e criação de pedidos.
    -   Gestão de múltiplos métodos de pagamento.

-   **📦 Gestão de Produtos para Vendedores:**
    -   CRUD completo de produtos.
    -   Upload de imagens para a nuvem (**Azure Blob Storage**).
    -   Aplicação de descontos (percentual ou valor fixo).

-   **📈 Dashboard Analítico para Vendedores:**
    -   Total de vendas do dia.
    -   Contagem de novos pedidos hoje.
    -   Gráfico de vendas mensais.
    -   Lista de produtos mais vendidos.
    -   Contagem de produtos em estoque.

## 🏗️ Arquitetura do Projeto

O projeto foi estruturado utilizando **Clean Architecture**, garantindo baixo acoplamento e alta coesão entre as camadas. Isso torna o sistema mais testável, flexível e fácil de manter.

-   **`Dominio`**: Camada mais interna. Contém as entidades de negócio (Usuário, Produto, Pedido), enums, objetos de valor e as interfaces dos repositórios. Não depende de nenhuma outra camada.
-   **`Aplicacao`**: Contém a lógica de negócio e os casos de uso (Use Cases). Orquestra o fluxo de dados entre a apresentação e o domínio, mas sem conhecer detalhes de implementação da UI ou do banco de dados.
-   **`Infraestrutura`**: Implementa as interfaces definidas no Domínio. É aqui que se encontram as configurações do Entity Framework, os repositórios, a implementação do `UnitOfWork`, a integração com o Azure Blob Storage e a geração de tokens JWT.
-   **`Apresentacao`**: A camada mais externa, responsável por expor a API (RESTful). Contém os Controllers, filtros de exceção e a configuração de injeção de dependência.

## 🛠️ Tecnologias e Padrões

| Categoria | Tecnologia / Padrão |
| :--- | :--- |
| **Framework & Linguagem** | `.NET 8`, `C# 11` |
| **Arquitetura** | `Clean Architecture`, `RESTful API`, `Domain-Driven Design (Lite)` |
| **Padrões de Projeto** | `Repository Pattern`, `Unit of Work`, `Injeção de Dependência` |
| **Banco de Dados** | `PostgreSQL`, `Entity Framework Core 8`, `FluentMigrator` (para migrations) |
| **Segurança** | `JWT (JSON Web Tokens)`, `BCrypt.Net` (para hashing de senhas) |
| **Armazenamento em Nuvem**| `Azure Blob Storage` (para imagens de produtos) |
| **Documentação da API** | `Swagger (OpenAPI)` |
| **Validações** | `FluentValidation` |

## 📚 Documentação da API

A documentação completa de todos os endpoints está disponível e pode ser testada via **Swagger** após iniciar a aplicação.

Acesse: **http://localhost:5000/swagger**

## 📫 Contato

**Luan Rodrigues Silva**

-   LinkedIn: https://www.linkedin.com/in/luanrodriguesconta
-   Email: luanrsilva209@gmail.com
