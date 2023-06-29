import { AxiosHeaders, InternalAxiosRequestConfig } from 'axios';
import config from '../env'

export const API_CONFIG: InternalAxiosRequestConfig = {
  baseURL: config.api.baseURL || import.meta.env.VITE_APP_BASE_API_URL,
  timeout: 5000,
  headers: new AxiosHeaders({
    'Content-Type': 'application/json'
  })
};
