import axios from 'axios';
import Resource from './resource'
import { VueCookieNext } from 'vue-cookie-next'
var author ='';
if(VueCookieNext.getCookie('login')!=undefined)
{
    author = 'bearer ' + VueCookieNext.getCookie('login');
}
export const API = axios.create({
  baseURL: Resource.Domain,
  headers: {
  'Content-Type': Resource.ContentType.json,
    'Authorization':author ,
    'httpOnly':true
  },
})