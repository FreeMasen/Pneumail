const path = require('path');

module.exports = function(env) {
    config = {
        entry: {
            app: path.join(__dirname, 'ts', 'app.tsx'),
            worker: path.join(__dirname, 'ts', 'services', 'worker.ts')
        },
        output: {
            path: path.join(__dirname, 'wwwroot', 'js'),
            filename: '[name].js',
            sourceMapFilename: '[name].js.map'
        },
        resolve: {
            extensions: ['.ts','.tsx','.js','.jsx', '.scss', '.css']
        },
        module: {
            rules: [
                {
                    test: /.tsx?$/,
                    exclude: ['node_modules'],
                    use: ['awesome-typescript-loader']
                }
            ]
        }
    };
    config.plugins = [
    ]
    if (env != 'prod') {
        config.devtool = 'eval-cheap-module-source-map';
    }
    return config;
}