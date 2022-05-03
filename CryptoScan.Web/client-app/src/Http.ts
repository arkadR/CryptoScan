export async function get(apiEndpoint: string) {
  return request(apiEndpoint, "GET");
}

export async function post(apiEndpoint: string, body: string = "") {
  return request(apiEndpoint, "POST", body);
}

export async function del(apiEndpoint: string, body: string = "") {
  return request(apiEndpoint, "DELETE", body);
}

export async function patch(apiEndpoint: string, body: string = "") {
  return request(apiEndpoint, "PATCH", body);
}

async function request(apiEndpoint: string, method: string, body: string = "") {
  return fetch(apiEndpoint, {
    method: method,
    headers: {
      'content-type': 'application/json;charset=UTF-8'
    }, 
    body: body});
}