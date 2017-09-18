const path = require('path');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = function(env) {
    extractSass = new ExtractTextPlugin(path.join(__dirname, 'css', '[name].css'));
    config = {
        entry: {
            app: path.join(__dirname, 'ts', 'app.tsx')
        },
        output: {
            path: path.join(__dirname, 'js'),
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
                },
                {
                    test: /\.scss$/,
                    use:
                        [
                            {loader: "style-loader"},
                            {loader: "css-loader"},
                            {loader: "sass-loader"}
                        ]
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