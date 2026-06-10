using Xunit;

// EF Core InMemory comparte estado a nivel de proceso; ejecutar los tests en serie
// evita interferencias entre clases que corren en paralelo.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
