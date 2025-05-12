import { environment } from "../../environments/environment";

const baseUrl = environment.apiUrl;

export const ApiEndpoints = {
    UrlShortener : `${baseUrl}/UrlShortener`
}