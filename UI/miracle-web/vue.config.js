module.exports = {
  transpileDependencies: ["vuetify"],
  devServer: {
    host: "127.0.0.1",
    port: 8083,
    /* proxy: "http://127.0.0.1:5103", */
  },
  chainWebpack: (config) => {
    config
      .plugin('html')
      .tap(args => {
        args[0].meta = {
          viewport: 'width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no, shrink-to-fit=no'
        };

        return args;
      })
  }
};
