const packageJson = require('../../package.json');

export const environment = {
    appInsights: {
        instrumentationKey: '0daee4e8-6df6-4782-a9ef-b46b134c1a34'
    },
    production: true,
    versions: {
        app: packageJson.version,
        angular: packageJson.dependencies['@angular/core'],
        ngrx: packageJson.dependencies['@ngrx/store'],
        material: packageJson.dependencies['@angular/material'],
        bootstrap: packageJson.dependencies.bootstrap,
        rxjs: packageJson.dependencies.rxjs,
        angularCli: packageJson.devDependencies['@angular/cli']
    }
};
