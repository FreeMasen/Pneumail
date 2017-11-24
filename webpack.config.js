const path = require('path');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = function(env) {
    extractSass = new ExtractTextPlugin(path.join(__dirname, 'css', '[name].css'));
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
        extractSass
    ]
    if (env != 'prod') {
        config.devtool = 'eval-cheap-module-source-map';
    }
    return config;
}