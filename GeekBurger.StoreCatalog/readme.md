# Entrega Final Microserviços
Daniel Souza Makiyama

## 1 - Criação
Criamos o APPService no Azure .
Criamos um repositorio no Azure Devops.    
Criamos os Workitens para dividir o desenvolvimento para a equipe.
    
## 2 - Entendimento
Utilizamos os projetos Products e LogMonitr para entendimento do funcionamento, 
pois a arquitetura é bastante diferente da habitual que os integrantes do grupos trabalham.

## 3 - Desenvolvimento

### Contract
Criado contratos seguindo documentação.
Publicado no Nuget do 14Fiap.
    
### BackgroundService
Iniciamos criando um facilitar para a implementação do IHostedService.
    
### Swagger
Configuração do swagger para expor as chamadas da API de uma maneira fácil.
    
### Repository 
Optamos por gravar os dados recuperados na memoria utilizando entity. UseInMemoryDatabase

### Build DEVOPS
Criamos uma pipeline para build no Azure Devops.
Foi necessário configurar o Nuget 14Net para subir no pipeline, o arquivo é NuGet.config.

### Release and Publish DEVOPS
Criamos uma publicação diretamente no Azure Devops a partir da geração do Artefato da Build.

### ServiceBus
Criado LogService para utilizar para logar erros e etapas no Topic Log.
Criado StoreCatalogReadyService para logar a mensagem quando o InitializeCheck termina.
Criado LessOfferService para regra especifica no Products.
Criado ProductChanged para assinar topico responsavel por avisar alterações de produtos.
Criado ProductionAreaChanged para assinar topico responsavel por avisar alterações de Areas de Produção.

### Controllers
implementado regras para Store e Products.

### Polly
Aplicado a Pattern WaitandRetry para todas as chamadas de API.

### Readme
Explicando nossos passos no desenvolvimento.