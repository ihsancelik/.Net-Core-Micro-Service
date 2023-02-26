<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" outlined elevation="5">
      <a @click="close(false)">
        <v-icon style="float: right;">mdi-close-circle</v-icon>
      </a>
      <h2 class="text-md-center">{{ translator("editUser") }}</h2>
      <hr class="mb-5" />

      <v-form>
        <v-row class="rows1">
          <v-col cols="12" md="8">
            <v-file-input
              prepend-icon=""
              prepend-inner-icon="mdi-camera"
              accept=".jpg, .png, .jpeg"
              :placeholder="translator('userImage')"
              @change="uploadImage"
            ></v-file-input>
          </v-col>

          <v-col cols="12" md="4">
            <img id="vimg" :src="userImage" class="profileImg" :alt="model.user.name" />
          </v-col>
        </v-row>

        <v-row class="rows">
          <v-col cols="12" md="6">
            <ValidationProvider #default="{ errors }" :name="translator('name')" rules="required|max:64">
              <v-text-field
                v-model="model.user.name"
                :label="translator('name')"
                :error-messages="errors"
                outlined
                counter="64"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
          <v-col cols="12" md="6">
            <ValidationProvider #default="{ errors }" :name="translator('surname')" rules="required|max:64">
              <v-text-field
                v-model="model.user.surname"
                :label="translator('surname')"
                :error-messages="errors"
                outlined
                counter="64"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
        </v-row>

        <v-row class="rows">
          <v-col cols="12" md="9">
            <ValidationProvider #default="{ errors }" :name="translator('username')" rules="required|max:32">
              <v-text-field
                v-model="model.user.username"
                :label="translator('username')"
                :error-messages="errors"
                outlined
                counter="32"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
          <v-col cols="12" md="2">
            <v-btn @click="passwordDialog" dark style="width: 100%;">
              <v-icon medium style="color: gold;">mdi-key</v-icon>
            </v-btn>
          </v-col>
          <v-col cols="12" md="1"></v-col>
        </v-row>

        <v-row class="rows">
          <v-col cols="12" md="8">
            <ValidationProvider #default="{ errors }" :name="translator('email')" rules="required|email">
              <v-text-field
                v-model="model.user.email"
                :label="translator('email')"
                outlined
                :error-messages="errors"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
          <v-col cols="12" md="4">
            <ValidationProvider
              #default="{ errors }"
              :name="translator('phone')"
              rules="required|min:10|max:11|numeric"
            >
              <v-text-field
                v-model="model.user.phoneNumber"
                :label="translator('phone')"
                :error-messages="errors"
                outlined
                counter="11"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
        </v-row>

        <v-row class="rows">
          <v-col cols="12" md="6">
            <ValidationProvider #default="{ errors }" :name="translator('roles')" rules="required">
              <v-select
                multiple
                v-model="model.roleIdList"
                :error-messages="errors"
                :items="roles"
                item-text="value"
                item-value="id"
                :placeholder="translator('selectRole')"
                outlined
                small-chips
                style="max-width: 400px;"
              ></v-select>
            </ValidationProvider>
          </v-col>
          <v-col cols="12" md="6">
            <v-select
              v-model="model.user.priorityId"
              :items="priorities"
              item-text="name"
              item-value="id"
              :placeholder="translator('selectPriority')"
              outlined
            ></v-select>
          </v-col>
        </v-row>

        <v-row class="rows">
          <v-col cols="12" md="8">
            <v-select
              v-model="model.user.companyId"
              :items="companies"
              item-text="name"
              item-value="id"
              :placeholder="translator('selectCompany')"
              outlined
            ></v-select>
          </v-col>
        </v-row>

        <v-switch v-model="model.user.isActive" :label="translator('isActive')"></v-switch>

        <hr class="mb-2" />

        <v-btn @click.prevent="userEdit" color="primary" dark class="v-btn--block v-size--large mb-2">
          {{ translator("update") }}
        </v-btn>
      </v-form>
    </v-card>

    <v-dialog v-model="dialogEnable" max-width="400" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </ValidationObserver>
</template>

<script>
import * as rules from "vee-validate/dist/rules";
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import { Base } from "@/helpers/RouteConstraints";
import MessageBox from "@/helpers/Components/MessageBox";
import PasswordReset from "../components/PasswordReset";
import CompanyService from "@/services/CompanyService";
import RoleService from "@/services/RoleService";
import PriorityService from "@/services/PriorityService";
import UserService from "@/services/UserService";

setInteractionMode("eager");
extend("required", { ...rules.required, message: "{_field_} can not be empty" });
extend("min", { ...rules.min, message: "{_field_} may not be less than {length} characters" });
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });
extend("numeric", { ...rules.numeric, message: "{_field_} can be numeric" });

export default {
  props: ["userIdProp"],
  data() {
    return {
      fieldStyle: "margin-bottom: -3%;margin-top: -3%;",
      userId: "",
      model: {
        user: {},
        roleIdList: [],
      },
      userImage: null,
      companies: [],
      priorities: [],
      roles: [],
      signalModel: {
        changes: false,
        returnValues: null,
      },

      dialogEnable: false,
      dialogKey: 0,
      selectedComponent: "",
      componentProps: null,
    };
  },
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  methods: {
    async initialize() {
      let userResponse = await new UserService().getById(this.userId);
      let imageResponse = await new UserService().userGetProfileImage(this.userId);
      let companyResponse = await new CompanyService().getListAll();
      let roleResponse = await new RoleService().getListAll();
      let priorityResponse = await new PriorityService().getListAll();

      if ((userResponse || imageResponse || companyResponse || roleResponse || priorityResponse) === 999)
        await this.initialize();

      this.model.user = userResponse.data;
      this.userImage = Base + imageResponse;
      this.companies = companyResponse.list;
      this.roles = roleResponse.list;
      this.priorities = priorityResponse.list;

      let userRoles = userResponse.data.user_Roles;

      userRoles.forEach((role) => {
        this.model.roleIdList.push(role.roleId);
      });
    },
    async userEdit() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let formData = new FormData();
        for (let [key, value] of Object.entries(this.model.user)) {
          formData.append(`${key}`, `${value}`);
        }
        formData.append("profilePhoto", this.model.user.profilePhoto);

        this.model.roleIdList.forEach((roleId) => {
          formData.append("roleIdList", roleId);
        });
        let response = await new UserService().edit(formData, this.userId);
        if (response.success) this.close(true);
        else {
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
      this.model.user.profilePhoto = image;
      let reader = new FileReader();
      reader.onload = function (e) {
        this.userImage = e.target.result.toString();
        document.getElementById("vimg").setAttribute("src", this.userImage);
      };
      reader.readAsDataURL(image);
    },
    passwordDialog() {
      this.selectedComponent = PasswordReset;
      this.componentProps = {
        userIdProp: this.userId,
      };

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
  created() {
    this.userId = this.userIdProp;
    this.initialize();
  },
};
</script>

<style scoped>
.rows {
  margin-bottom: -6%;
}

.rows1 {
  margin-bottom: -2%;
}

.profileImg {
  display: inline-block;
  background-size: contain;
  max-height: 65px;
  height: 65px;
  margin-left: 5%;
}
</style>
