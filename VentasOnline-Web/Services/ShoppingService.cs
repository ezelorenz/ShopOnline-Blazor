using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using VentasOnline.Models.Dto;

namespace VentasOnlineWeb.Services
{
    public class ShoppingService : IShoppingService
    {
        private readonly HttpClient _httpClient;

        public ShoppingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public event Action<int> OnShoppingCartChanged;

        public async Task<List<CartItemDto>> GetItems(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Shopping/{userId}/GetItems");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CartItemDto>().ToList();
                    }
                    return await response.Content.ReadFromJsonAsync<List<CartItemDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status: {response.StatusCode} Message: {message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<CartItemToAddDto>("api/Shopping", cartItemToAddDto);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(CartItemDto);
                    }
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status: {response.StatusCode} Message: {message}");
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<CartItemDto> DeleteItem(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Shopping/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                return default(CartItemDto);
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

        public async Task<CartItemDto> DeleteCartItem(int cartId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Shopping/DeleteCartItem/{cartId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                return default(CartItemDto);
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

        public async Task<CartItemDto> UpdateQuantity(CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                var jsonRequest = JsonConvert.SerializeObject(cartItemQtyUpdateDto);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await _httpClient.PatchAsync($"api/Shopping/{cartItemQtyUpdateDto.CartItemId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                return null;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void RaiseEventOnShoppingCartChanged(int totalQuantity)
        {
            if(OnShoppingCartChanged != null)
            {
                OnShoppingCartChanged.Invoke(totalQuantity);
            }
        }

        
    }
}
