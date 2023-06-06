import axios, { AxiosInstance, AxiosResponse, InternalAxiosRequestConfig } from 'axios';
import { PagedList } from '~/app/modules/core/domain/models/PagedList';

export interface ApiConfig extends InternalAxiosRequestConfig {}

export interface ApiResponse {
  isSuccess: boolean;
  statusCode: number;
  errors: string[];
}

export interface PaginationResponse<T> {
  items: T;
  metadata: PagedList;
}

export interface ApiDataResponse<T> extends ApiResponse {
  result: T;
}

export class Api {
  private axiosInstance: AxiosInstance;

  constructor(config: ApiConfig) {
    this.axiosInstance = axios.create(config);

    // Add a request interceptor
    this.axiosInstance.interceptors.request.use(
      function (config: ApiConfig) {
        // Do something before request is sent
        return config;
      },
      function (error) {
        // Do something with request error
        return Promise.reject(error);
      }
    );
  
    // Add a response interceptor
    this.axiosInstance.interceptors.response.use(
      function (response: AxiosResponse) {
        return response.data;
      },
      function (error) {
        return Promise.reject(error);
      }
    );
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public get<T>(url: string, config?: any): Promise<T> {
    return this.axiosInstance.get(url, config);
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public post<T>(url: string, data?: any, config?: any): Promise<T> {
    return this.axiosInstance.post(url, data, config);
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public delete<T>(url: string, config?: any): Promise<T> {
    return this.axiosInstance.delete(url, config);
  }

  // Thêm các phương thức khác cho các HTTP method như put, delete, patch, ...
}