import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'BankApp',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44327/',
    redirectUri: baseUrl,
    clientId: 'BankApp_App',
    responseType: 'code',
    scope: 'offline_access BankApp',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44327',
      rootNamespace: 'BankApp',
    },
  },
} as Environment;
