<template>
  <ValidationObserver ref="observer">
    <v-container class="mt-8">
      <v-card class="pa-10 pt-10 mt-10" outlined elevation="5">
        <v-form>
          <v-row id="rows1" style="height: 130px;">
            <v-sheet class="v-sheet--offset mx-auto" color="transparent">
              <div class="d-flex grow flex-wrap">
                <div
                  class="v-avatar mx-auto v-card--material__avatar elevation-5 grey"
                  style="height: 200px; min-width: 200px; width: 200px;"
                >
                  <div class="v-responsive v-image">
                    <div class="v-responsive__sizer" style="padding-bottom: 100%;"></div>
                    <div
                      id="userimg"
                      class="v-image__image v-image__image--cover"
                      :style="'background-image: url(' + user.img + ');background-position: center center;'"
                    ></div>
                    <div class="v-responsive__content" style="width: 500px;"></div>
                  </div>
                </div>
              </div>
            </v-sheet>
          </v-row>

          <v-col cols="12" md="12">
            <ValidationProvider #default="{ errors }" :name="translator('userImage')">
              <v-file-input
                hide-input
                false
                prepend-inner-icon="mdi-camera"
                accept=".jpg, .png, .jpeg"
                :placeholder="translator('userImage')"
                @change="uploadImage"
                :error-messages="errors"
              ></v-file-input>
            </ValidationProvider>
          </v-col>

          <v-row id="rows">
            <v-col cols="12" md="6">
              <ValidationProvider #default="{ errors }" :name="translator('name')" rules="required|max:32">
                <v-text-field
                  v-model="user.name"
                  :label="translator('name')"
                  :error-messages="errors"
                  outlined
                  counter="32"
                ></v-text-field>
              </ValidationProvider>
            </v-col>

            <v-col cols="12" md="6">
              <ValidationProvider #default="{ errors }" :name="translator('surname')" rules="required|max:32">
                <v-text-field
                  v-model="user.surname"
                  :label="translator('surname')"
                  :error-messages="errors"
                  outlined
                  counter="32"
                ></v-text-field>
              </ValidationProvider>
            </v-col>
          </v-row>

          <v-row id="rows">
            <v-col cols="12" md="10">
              <ValidationProvider #default="{ errors }" :name="translator('email')" rules="required|email">
                <v-text-field
                  v-model="user.email"
                  :label="translator('email')"
                  outlined
                  :error-messages="errors"
                ></v-text-field>
              </ValidationProvider>
            </v-col>
            <v-col cols="12" md="2" class="pt-5">
              <v-btn @click="passwordDialog" dark style="width: 100%;">
                <v-icon medium style="color: gold;">mdi-key</v-icon>
              </v-btn>
            </v-col>
          </v-row>

          <v-row id="rows">
            <v-col cols="12" md="12">
              <ValidationProvider
                #default="{ errors }"
                :name="translator('phone')"
                rules="required|min:10|max:11|numeric"
              >
                <v-text-field
                  v-model="user.phoneNumber"
                  :label="translator('phone')"
                  :error-messages="errors"
                  outlined
                  counter="11"
                ></v-text-field>
              </ValidationProvider>
            </v-col>
          </v-row>

          <hr class="mb-3" />

          <v-btn @click.prevent="userUpdate" color="primary" dark class="v-btn--block v-size--large mb-2">
            {{ translator("update") }}
          </v-btn>
        </v-form>
      </v-card>
    </v-container>
    <v-dialog v-model="dialogEnable" max-width="400" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </ValidationObserver>
</template>

<script>
/* eslint-disable */
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import { CoreBase } from "@/helpers/RouteConstraints";
import * as rules from "vee-validate/dist/rules";
import MessageBox from "@/helpers/components/MessageBox";
import PasswordReset from "../Settings/components/PasswordReset";
import UserService from "@/services/UserService";
import { ReturnConstraints } from "@/helpers/Constraints";

setInteractionMode("eager");
extend("required", { ...rules.required, message: "{_field_} can not be empty" });
extend("min", { ...rules.min, message: "{_field_} may not be less than {length} characters" });
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });
extend("numeric", { ...rules.numeric, message: "{_field_} can be numeric" });

export default {
  props: ["userIdProp"],
  data() {
    return {
      userId: 0,
      user: {},
      userImage: null,

      dialogEnable: false,
      dialogKey: 0,
      selectedComponent: "",
      componentProps: null,

      signalModel: {
        changes: false,
        returnValues: null,
      },
    };
  },
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  created() {
    this.initialize();
    this.userId = this.userIdProp;
  },
  methods: {
    async initialize() {
      let userResponse = await new UserService().getOutSource();
      if (userResponse === ReturnConstraints.REFRESH) await this.initialize();

      let imageResponse = await new UserService().getImage();
      const user = userResponse.data;
      user.img = null;
      user.img = CoreBase + imageResponse.data;
      this.user = user;
    },
    async userUpdate() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let formData = new FormData();

        for (let [key, value] of Object.entries(this.user)) {
          formData.append(`${key}`, `${value}`);
        }

        formData.append("ProfilePhoto", this.user.ProfilePhoto);

        let response = await new UserService().updateOutSource(formData);
        if (response.success) {
          this.selectedComponent = MessageBox;
          this.componentProps = {
            messageTitleProp: this.translator("success"),
            messageTextProp: response.message,
          };
          this.dialogEnable = true;
          this.dialogKey += 1;
          await this.initialize();
        } else {
          this.selectedComponent = MessageBox;
          this.componentProps = {
            messageTitleProp: this.translator("failed"),
            messageTextProp: response.message,
          };
          this.dialogEnable = true;
          this.dialogKey += 1;
        }
      }
    },
    uploadImage(image) {
      this.user.ProfilePhoto = image;
      let reader = new FileReader();
      reader.onload = function (e) {
        document.getElementById("userimg").setAttribute("src", e.target.result.toString());
      };
      reader.readAsDataURL(image);
    },
    passwordDialog() {
      this.selectedComponent = PasswordReset;
      this.componentProps = { userIdProp: this.userId };

      this.dialogEnable = true;
    },
    closeDialog() {
      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
    },
  },
};
</script>

<style scoped>
#userimg .v-responsive__content {
  width: 100% !important;
}
.cardStyle {
  height: 175px;
  width: 100%;
  margin-bottom: 25px;
}
.v-sheet--offset {
  top: -100px;
  position: relative;
  max-width: calc(100% - 32px);
}
.v-sheet--offset div {
  font-size: 75px;
  text-align: center;
}
</style>
