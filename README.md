### 展才网

1. 任务规划

莫雷：负责前端，主要管理View、Content文件夹里的静态资源。
小辉：主要负责Model层，数据库管理。
先修客：负责后端，提供接口，返回数据json格式

2. 接口调用 本接口只提供同域名调用，不支持跨域调用。

> 统一调用格式

```
{
    "status": 0,
    "msg": "获取成功",
    "data": [
        {
            "linkID": 1,
            "linkName": "投融百科",
            "linkUrl": "http: //www.trjcn.com/baike.html",
            "isShow": true,
            "isDelete": false,
            "addTime": null,
            "updateTime": null
        },
        {
            "linkID": 2,
            "linkName": "投融界专题",
            "linkUrl": "http: //www.trjcn.com/tgzt/index.html",
            "isShow": true,
            "isDelete": false,
            "addTime": null,
            "updateTime": null
        },
        {
            "linkID": 3,
            "linkName": "金价查询",
            "linkUrl": "http: //www.dyhjw.com/",
            "isShow": true,
            "isDelete": false,
            "addTime": null,
            "updateTime": null
        }
    ]
}
```

#### 获取友情链接 

```
Get

demo /PubilcApi/GetLinks
```

#### 获取验证码

```
Get

demo /PubilcApi/GetVcode?codelength=4&codesize=20
```

参数       | 数据类型 | 说明
-----------|---------|----
codelength | int     | 验证码长度
codesize   | float   | 验证码字体大小

#### 获取视频列表

```
Get

demo /PubilcApi/GetVideoList?pageindex=1&pagesize=5
```

参数       | 数据类型 | 说明
-----------|---------|----
pageindex  | int     | 查询第几页的数据
pagesize   | int     | 每页显示几条数据


#### 获取视频详细

```
Get

demo /PubilcApi/GetVideoDetail?id=1
```

参数       | 数据类型 | 说明
-----------|---------|----
id         | int     | 视频编号id


#### 会员注册

```
Post

demo /PubilcApi/PostRegister

params
{
    "vcode":"vcode",
    "email":"email",
    "password":"password"
}
```

参数       | 数据类型 | 说明
-----------|---------|----
vcode      | string  | 验证码
email      | string  | 邮箱
password   | string  | 密码

#### 会员登录

```
Post

demo /PubilcApi/PostLogin

params
{
    "vcode":"vcode",
    "email":"email",
    "password":"password"
}
```

参数       | 数据类型 | 说明
-----------|---------|----
vcode      | string  | 验证码
email      | string  | 邮箱
password   | string  | 密码