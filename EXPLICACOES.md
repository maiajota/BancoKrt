Minha solução para evitar custos desnecessários com consultas à AWS seria a implementação de cache. Dessa forma, ao consultar uma conta pela 1° vez passando pela AWS, não seria necessário acessar o banco novamente no prazo estipulado (no meu caso 15 minutos, mas normalmente 24 horas).

Ao invés de efetuar a exclusão das contas, eu optei por um soft-delete, pois é crucial para um sistema crítico manter as informações das contas não mais atuantes, evitando problemas de consultas no futuro.


