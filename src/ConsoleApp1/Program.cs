using Binance.Net.Objects;
using Ligric.CryptoObserver;

const string PRIVATE_KEY = "651096d67c3d1a080daf6d26a37ad545864d312b7a6b24d5f654d4f26a1a7ddc";
const string PUBLICK_KEY = "c58577a8b8d83617fb678838fa8e43c83e53384e88fef416c81658e51c6c48f3";

BinanceFuturesManager futures = new BinanceFuturesManager(new BinanceApiCredentials(PUBLICK_KEY, PRIVATE_KEY));

await futures.AttachOrdersSubscribtionsAsync();
