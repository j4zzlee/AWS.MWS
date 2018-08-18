using Claytondus.AmazonMWS.Products;
using Claytondus.AmazonMWS.Products.Model;
using Claytondus.AmazonMWS.Feeds;
using System;
using System.Linq;
using System.Collections.Generic;

namespace st2forget.AWS.MWS
{
    public class ProductsAPI
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly string _merchantId;
        private readonly string _marketPlaceId;
        private readonly string _serviceUrl;

        public ProductsAPI(
            string accessKey, 
            string secretKey, 
            string merchantId,
            string marketPlaceId,
            string serviceUrl)
        {
            _accessKey = accessKey;
            _secretKey = secretKey;
            _merchantId = merchantId;
            _marketPlaceId = marketPlaceId;
            _serviceUrl = serviceUrl;
        }

        public object GetBySKU(string sku)
        {
            var client = new MarketplaceWebServiceProductsClient(_accessKey, _secretKey);
            var resp = client.GetMatchingProduct(new GetMatchingProductRequest(
                _merchantId,
                _marketPlaceId,
                new ASINListType { ASIN = new List<string> { sku } }));
            var product = resp.GetMatchingProductResult.FirstOrDefault();
            if (product?.Error != null)
            {
                throw new Exception(product.Error.Detail.Any.FirstOrDefault()?.ToString());
            }
            return product?.Product;
        }

        public string Create()
        {
            var client = new MarketplaceWebServiceClient(_accessKey, _secretKey, new MarketplaceWebServiceConfig
            {
                ServiceURL = _serviceUrl
            });
            var req = new Claytondus.AmazonMWS.Feeds.Model.SubmitFeedRequest();
            req.Merchant = _merchantId;
            req.WithMarketplaceIdList(new Claytondus.AmazonMWS.Feeds.Model.IdList { Id = new List<string> { } });
            var res = client.SubmitFeed(req);
            return string.Empty;
        }
    }
}
