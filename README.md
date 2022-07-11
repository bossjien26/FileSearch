# File Search


### test

> appsettings.test.json need copy to __**bin/Debug/net{version}**__


## k8s

```sh
minikube start

minikube docker-env
export DOCKER_TLS_VERIFY="1"
export DOCKER_HOST="tcp://172.17.0.1"
export DOCKER_CERT_PATH="/Users/oufukugi/.minikube/certs"
export MINIKUBE_ACTIVE_DOCKERD="minikube"
```

```sh
#enable ingress
minikube addons enable ingress
```

[install nginx ingress](https://dev.to/christianzink/how-to-build-an-asp-net-core-kubernetes-microservices-architecture-with-angular-on-local-docker-desktop-using-ingress-395n)
 ```sh
#install nginx
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v0.41.2/deploy/static/provider/cloud/deploy.yaml

#uninstall nginx
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v0.41.2/deploy/static/provider/cloud/deploy.yaml
```

### forward port
```sh
kubectl port-forward file-search-pod 5000:80
```


```sh
minikube dashboard --url
```

```sh
kubectl run test --image=file_search_api --restart=Never --image-pull-policy=Never
```


```js
getIPs().then(res => document.write(res.join('\n')))
<script src="https://cdn.jsdelivr.net/gh/joeymalvinni/webrtc-ip/dist/bundle.dev.js"></script>
```