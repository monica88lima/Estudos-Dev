Feature: Distribuição de eventos recebidos do SQS - Bloqueio

  Scenario: Evento deve ser enviado ao SNS de bloqueio com dados formatados corretamente
    Given que recebo um evento do SQS do tipo "bloqueio"
    When a lambda processa o evento
    Then os dados devem ser enviados para o tópico "enviar-dados-bloqueio"
    And a formatacao da data deve estar no formato aaaa-mm-dd "2023-10-01"
    And a formatacao da hora deve estar no formato hh:mm "15:40"
    And a formatacao da data de criacao deve estar no formato aaaa-mm-dd "2026-01-30T13:00:00"