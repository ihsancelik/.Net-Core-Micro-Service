<template>
  <v-app>
    <!--Menu Component-->
    <app-bar />

    <v-content>
      <Header
        v-if="$route.name !== 'home' && $route.name !== 'miracle-world'"
        :title="translator($route.name.replace('-', '_'))"
      />
      <v-container>
        <v-row justify="center" class="justify-md-center">
          <v-col cols="12" md="12">
            <v-fade-transition mode="out-in">
              <router-view />
            </v-fade-transition>
          </v-col>
        </v-row>
      </v-container>
    </v-content>

    <v-footer app height="50" width="100%" style="float: right;">
      <span class="px-4">&copy; {{ new Date().getFullYear() }}</span>
      <v-row>
        <v-col cols="12" md="7">
          <div style="font-variant: small-caps;">
            <a href="https://software.test.com/PrivacyPolicy.pdf" target="_blank" style="color: black;">
              {{ translator("privacyPolicy") }}
            </a>

            <a
              href="https://software.test.com/CancellationReturnsPolicy.pdf"
              target="_blank"
              style="color: black; margin-right: 1%;"
            >
              | {{ translator("cancellationPolicy") }}
            </a>
          </div>
        </v-col>
        <v-col cols="12" md="5">
          <v-btn @click="chat" class="chatIcon">
            <v-img
              :src="openIcon"
              class="chatImage"
            />
          </v-btn>
        </v-col>
      </v-row>
    </v-footer>

    <v-dialog v-model="dialogEnable" :max-width="dialogWidth" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog"></component>
    </v-dialog>
  </v-app>
</template>

<script>
import AppBar from "../components/Header/AppBar";
import Header from "../components/Header/Header";
import MessageBox from "../helpers/components/MessageBox";
import LiveTicket from "../pages/LiveTicket/LiveTicket";
export default {
  name: "Index",
  components: {
    AppBar,
    Header,
  },
  data() {
    return {
      dialogEnable: false,
      dialogKey: 0,
      dialogWidth: null,
      selectedComponent: "",
      componentProps: null,
      openIcon: "./chatIcon.jpg",
    };
  },
  methods: {
    chat() {
      this.selectedComponent = LiveTicket;
      this.componentProps = { icon: this.openIcon };

      this.openDialog(550);
    },
    openDialog(width = 400) {
      this.dialogEnable = true;
      this.dialogWidth = width;
    },
    closeDialog(signalModel) {
      const changes = signalModel.changes;
      const returnValues = signalModel.returnValues;
      if (changes) {
        this.setReturnValues(returnValues);
        this.initialize();
      }
      if (this.selectedComponent === MessageBox) {
        if (changes) this.deleteProduct(this.selectedProduct);
      }

      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    setReturnValues(values) {
      return values;
    },
  },
};
</script>

<style scoped>
a {
  text-decoration: none;
}
.v-dialog:not(.v-dialog--fullscreen) {
  bottom: 0 !important;
  right: 0 !important;
  position: absolute !important;
}
.chatIcon {
  float: right;
  height: 90px;
  width: 90px;
  margin-top: -10px;
  margin-right: 4px;
  background-color: transparent;
}
.chatImage {
  float: right; 
  height: 50px; 
  width: 50px; 
  margin-top: -9px; 
  margin-right: 4px;
  background-color: transparent;
}
</style>
