const path = require('path');

module.exports = {
  module: {
    rules: [
      {
        test: /\.(js)$/,
        exclude: /node_modules/,
        use: {
          loader: 'babel-loader'
        }
      }
    ]
  },
  output: {
    path: path.resolve(__dirname, '../wwwroot/js'),
    filename: 'js-lib.js',
    library: 'FrontNetJS'
  }
};
