module.exports = {
  "transpileDependencies": [
    "vuetify"
  ],
  configureWebpack: {
    devtool: 'source-map'
  },
  publicPath: process.env.NODE_ENV === 'production'
      ? '/'
      : '/'

  /*devServer: {
    proxy: {
      "/api": {
        target: "https://localhost:5001",
        changeOrigin: true,
        https: true
      }
    }
  }*/
}