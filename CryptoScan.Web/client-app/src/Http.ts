export async function get(apiEndpoint: string) {
  return request(apiEndpoint, "GET");
}

export async function post(apiEndpoint: string, body: string | null = null) {
  return request(apiEndpoint, "POST", body);
}

export async function del(apiEndpoint: string, body: string | null = null) {
    return request(apiEndpoint, "DELETE", body);
}

async function request(apiEndpoint: string, method: string, body: string | null = null) {
  return fetch(apiEndpoint, {
    method: method,
    headers: {
      'content-type': 'application/json;charset=UTF-8'
    }, 
    body: body});
}