<template>
  <v-container fluid>
    <v-row style="margin-bottom: 1%;">
      <v-card class="mx-auto" min-width="600px" max-width="900" :raised="true" elevation="10">
        <v-card-title style="justify-content: center;">
          <v-img width="100%" max-width="300px" src="/mw.png" />
        </v-card-title>
        <br />
        <v-card-subtitle class="pb-0 text-center" style="font-size: 34px;">Miracle World</v-card-subtitle>
        <br />
        <v-card-text class="text--primary">
          <v-container fluid>
            <v-row>
              <v-col cols="12">
                <v-card class="ma-3 pa-6" height="250">
                  <v-card-text class="text-center">
                    <div style="margin-bottom: 50px;">
                      <v-btn
                        style="font-size: 25px; padding: 25px;"
                        outlined
                        color="primary"
                        @click="getMiracleWorld(detectedPlatform)"
                        >{{ translator("downloadNow").toUpperCase() }}</v-btn
                      >
                      <br />
                      <span style="font-size: 10px; font-weight: 700;"
                        >{{ translator("detectedPlatform") }} : {{ detectedPlatform }}</span
                      >
                    </div>

                    <v-select
                      v-model="selectedPlatform"
                      item-text="value"
                      item-value="value"
                      :items="platforms"
                      outlined
                      dense
                      :placeholder="translator('specificDownload')"
                    ></v-select>
                  </v-card-text>
                </v-card>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>

        <hr style="margin-left: 30px; margin-right: 30px;" />
      </v-card>
    </v-row>
  </v-container>
</template>

<script>
import ProductService from "@/services/ProductService";
export default {
  data() {
    return {
      detectedPlatform: "",
      selectedPlatform: "",
      platforms: [{ value: "Linux" }, { value: "Mac" }, { value: "Windows" }],
    };
  },
  methods: {
    initialize() {
      if (navigator.userAgent.toLowerCase().includes("windows")) this.detectedPlatform = "Windows";
      else if (navigator.userAgent.toLowerCase().includes("mac")) this.detectedPlatform = "Mac";
      else if (navigator.userAgent.toLowerCase().includes("linux")) this.detectedPlatform = "Linux";
      else alert("Invalid");
    },
    async getMiracleWorld(platform) {
      let response = await new ProductService().getMiracleWorld(platform);
      if (response !== undefined && response !== "") {
        let file_path = response;
        let downloadLink = document.createElement("a");
        downloadLink.href = file_path;
        downloadLink.download = file_path;
        document.body.appendChild(downloadLink);
        downloadLink.click();
        document.body.removeChild(downloadLink);
        this.selectedPlatform = "";
      }
    },
  },
  watch: {
    selectedPlatform() {
      this.getMiracleWorld(this.selectedPlatform);
    },
  },
  created() {
    this.initialize();
  },
};
</script>

<style scoped></style>
