import devConfig from "./environments/dev";
import prodConfig from "./environments/prod";

const env: 'development' | 'production' = import.meta.env.NODE_ENV || 'development';

const config = {
  development: {
    ...devConfig
  },
  production: {
    ...prodConfig
  },
};

export default config[env];
