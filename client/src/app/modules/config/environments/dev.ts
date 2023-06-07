const devConfig = {
  api: {
    baseURL: import.meta.env.VITE_APP_BASE_API_URL,
  },
  logger: {
    level: 'debug',
  },
  features: {
    enableFeatureA: true,
    enableFeatureB: false,
  },
};

export default devConfig;
