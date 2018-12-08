'use strict';
/*wxopen - yiluomyt*/
/**
 * 
 * @class Qincai
 * @param {string} [domain] - The project domain
 */
export default class Qincai {

    constructor(domain) {
        this.domain = domain !== undefined ? domain : 'http://localhost:5000';
        if (this.domain.length === 0) {
            throw new Error('Domain must be specified as a string.');
        }
        this.authenticate = function() {
            return new Promise((resolve, reject) => {
                reject(new Error('Please Set authenticate First.'));
            })
        };
    }

    /**
     * Set authenticate method
     * @method
     * @name Qincai#setAuthenticate
     * @param {function} auth - authenticate method
     */
    setAuthenticate(auth) {
        this.authenticate = auth;
    }

    /**
     * HTTP Request
     * @method
     * @name Qincai#request
     * @param {string} method - HTTP 请求方法
     * @param {string} url - 开发者服务器接口地址
     * @param {object} data - 请求的参数
     * @param {object} headers - 设置请求的 header ,默认为 application/json
     */
    request(method, url, parameters, data, headers) {
        return new Promise((resolve, reject) => {
            wx.request({
                url: url,
                data: data,
                header: headers,
                method: method,
                success: res => {
                    if (res.statusCode >= 200 && res.statusCode <= 299) {
                        resolve(res.data)
                    } else {
                        reject(res)
                    }
                },
                fail: e => reject(e)
            })
        })
    };

    /**
     * 我的回答
     * @method
     * @name Qincai#ListMyAnswers
     * @param {object} parameters - method options and parameters
     * @param {integer} parameters.page - 当前页数
     * @param {integer} parameters.pageSize - 每页数量
     * @param {string} parameters.orderBy - 排序字段
     * @param {boolean} parameters.descending - 是否降序
     */
    ListMyAnswers(parameters) {
        if (parameters === undefined) {
            parameters = {};
        }
        let domain = this.domain,
            path = '/api/Answer/me';
        let data = {},
            headers = {};

        headers['Accept'] = 'application/json';
        headers['Content-Type'] = '';

        if (parameters['page'] !== undefined) {
            data['Page'] = parameters['page'];
        }

        if (parameters['pageSize'] !== undefined) {
            data['PageSize'] = parameters['pageSize'];
        }

        if (parameters['orderBy'] !== undefined) {
            data['OrderBy'] = parameters['orderBy'];
        }

        if (parameters['descending'] !== undefined) {
            data['Descending'] = parameters['descending'];
        }

        return new Promise((resolve, reject) => {
            this.authenticate()
                .then(token => {
                    headers['Authorization'] = 'Bearer ' + token;
                    resolve(this.request('GET', domain + path, parameters, data, headers))
                })
        })
    };
    /**
     * 删除回答
     * @method
     * @name Qincai#DeleteAnswer
     * @param {object} parameters - method options and parameters
     * @param {string} parameters.id - 回答Id
     */
    DeleteAnswer(parameters) {
        if (parameters === undefined) {
            parameters = {};
        }
        let domain = this.domain,
            path = '/api/Answer/{id}';
        let data = {},
            headers = {};

        headers['Accept'] = 'application/json';
        headers['Content-Type'] = '';

        path = path.replace('{id}', parameters['id']);

        if (parameters['id'] === undefined) {
            return new Promise((resolve, reject) => {
                reject(new Error('Missing required  parameter: id'));
            });
        }

        return new Promise((resolve, reject) => {
            this.authenticate()
                .then(token => {
                    headers['Authorization'] = 'Bearer ' + token;
                    resolve(this.request('DELETE', domain + path, parameters, data, headers))
                })
        })
    };
    /**
     * 返回图片上传Token
     * @method
     * @name Qincai#GetUploadToken
     * @param {object} parameters - method options and parameters
     */
    GetUploadToken(parameters) {
        if (parameters === undefined) {
            parameters = {};
        }
        let domain = this.domain,
            path = '/api/Image/UploadToken';
        let data = {},
            headers = {};

        headers['Accept'] = 'application/json';
        headers['Content-Type'] = '';

        return new Promise((resolve, reject) => {
            this.authenticate()
                .then(token => {
                    headers['Authorization'] = 'Bearer ' + token;
                    resolve(this.request('GET', domain + path, parameters, data, headers))
                })
        })
    };
    /**
     * 后期需要按某一字段排序
     * @method
     * @name Qincai#ListQuestions
     * @param {object} parameters - method options and parameters
     * @param {string} parameters.search - 搜索关键词
     * @param {string} parameters.category - 所需查找问题的分类
     * @param {string} parameters.userId - 提问者的Id
     * @param {integer} parameters.page - 当前页数
     * @param {integer} parameters.pageSize - 每页数量
     * @param {string} parameters.orderBy - 排序字段(*QuestionTime|LastTime)
     * @param {boolean} parameters.descending - 是否降序
     */
    ListQuestions(parameters) {
        if (parameters === undefined) {
            parameters = {};
        }
        let domain = this.domain,
            path = '/api/Question';
        let data = {},
            headers = {};

        headers['Accept'] = 'application/json';
        headers['Content-Type'] = '';

        if (parameters['search'] !== undefined) {
            data['Search'] = parameters['search'];
        }

        if (parameters['category'] !== undefined) {
            data['Category'] = parameters['category'];
        }

        if (parameters['userId'] !== undefined) {
            data['UserId'] = parameters['userId'];
        }

        if (parameters['page'] !== undefined) {
            data['Page'] = parameters['page'];
        }

        if (parameters['pageSize'] !== undefined) {
            data['PageSize'] = parameters['pageSize'];
        }

        if (parameters['orderBy'] !== undefined) {
            data['OrderBy'] = parameters['orderBy'];
        }

        if (parameters['descending'] !== undefined) {
            data['Descending'] = parameters['descending'];
        }

        return this.request('GET', domain + path, parameters, data, headers);
    };
    /**
     * 我的问题
     * @method
     * @name Qincai#ListMyQuestions
     * @param {object} parameters - method options and parameters
     * @param {integer} parameters.page - 当前页数
     * @param {integer} parameters.pageSize - 每页数量
     * @param {string} parameters.orderBy - 排序字段(*QuestionTime|LastTime)
     * @param {boolean} parameters.descending - 是否降序
     */
    ListMyQuestions(parameters) {
        if (parameters === undefined) {
            parameters = {};
        }
        let domain = this.domain,
            path = '/api/Question/me';
        let data = {},
            headers = {};

        headers['Accept'] = 'application/json';
        headers['Content-Type'] = '';

        if (parameters['page'] !== undefined) {
            data['Page'] = parameters['page'];
        }

        if (parameters['pageSize'] !== undefined) {
            data['PageSize'] = parameters['pageSize'];
        }

        if (parameters['orderBy'] !== undefined) {
            data['OrderBy'] = parameters['orderBy'];
        }

        if (parameters['descending'] !== undefined) {
            data['Descending'] = parameters['descending'];
        }

        return new Promise((resolve, reject) => {
            this.authenticate()
                .then(token => {
                    headers['Authorization'] = 'Bearer ' + token;
                    resolve(this.request('GET', domain + path, parameters, data, headers))
                })
        })
    };
    /**
     * 根据Id获取问题
     * @method
     * @name Qincai#GetQuestionById
     * @param {object} parameters - method options and parameters
     * @param {string} parameters.id - 问题Id
     */
    GetQuestionById(parameters) {
        if (parameters === undefined) {
            parameters = {};
        }
        let domain = this.domain,
            path = '/api/Question/{id}';
        let data = {},
            headers = {};

        headers['Accept'] = 'application/json';
        headers['Content-Type'] = '';

        path = path.replace('{id}', parameters['id']);

        if (parameters['id'] === undefined) {
            return new Promise((resolve, reject) => {
                reject(new Error('Missing required  parameter: id'));
            });
        }

        return this.request('GET', domain + path, parameters, data, headers);
    };
    /**
     * 删除问题
     * @method
     * @name Qincai#DeleteQuestion
     * @param {object} parameters - method options and parameters
     * @param {string} parameters.id - 问题Id
     */
    DeleteQuestion(parameters) {
        if (parameters === undefined) {
            parameters = {};
        }
        let domain = this.domain,
            path = '/api/Question/{id}';
        let data = {},
            headers = {};

        headers['Accept'] = 'application/json';
        headers['Content-Type'] = '';

        path = path.replace('{id}', parameters['id']);

        if (parameters['id'] === undefined) {
            return new Promise((resolve, reject) => {
                reject(new Error('Missing required  parameter: id'));
            });
        }

        return new Promise((resolve, reject) => {
            this.authenticate()
                .then(token => {
                    headers['Authorization'] = 'Bearer ' + token;
                    resolve(this.request('DELETE', domain + path, parameters, data, headers))
                })
        })
    };
    /**
     * 问题的回答列表
     * @method
     * @name Qincai#ListAnswersByQuestionId
     * @param {object} parameters - method options and parameters
     * @param {string} parameters.id - 问题Id
     * @param {integer} parameters.page - 当前页数
     * @param {integer} parameters.pageSize - 每页数量
     * @param {string} parameters.orderBy - 排序字段
     * @param {boolean} parameters.descending - 是否降序
     */
    ListAnswersByQuestionId(parameters) {
        if (parameters === undefined) {
            parameters = {};
        }
        let domain = this.domain,
            path = '/api/Question/{id}/Answers';
        let data = {},
            headers = {};

        headers['Accept'] = 'application/json';
        headers['Content-Type'] = '';

        path = path.replace('{id}', parameters['id']);

        if (parameters['id'] === undefined) {
            return new Promise((resolve, reject) => {
                reject(new Error('Missing required  parameter: id'));
            });
        }

        if (parameters['page'] !== undefined) {
            data['Page'] = parameters['page'];
        }

        if (parameters['pageSize'] !== undefined) {
            data['PageSize'] = parameters['pageSize'];
        }

        if (parameters['orderBy'] !== undefined) {
            data['OrderBy'] = parameters['orderBy'];
        }

        if (parameters['descending'] !== undefined) {
            data['Descending'] = parameters['descending'];
        }

        return this.request('GET', domain + path, parameters, data, headers);
    };
    /**
     * 创建一个新的问题
     * @method
     * @name Qincai#CreateQuestion
     * @param {object} parameters - method options and parameters
     * @param {} parameters.dto - 新问题
     */
    CreateQuestion(parameters) {
        if (parameters === undefined) {
            parameters = {};
        }
        let domain = this.domain,
            path = '/api/Question/CreateQuestion';
        let data = {},
            headers = {};

        headers['Accept'] = 'application/json';
        headers['Content-Type'] = 'application/json-patch+json,application/json,text/json,application/*+json';

        if (parameters['dto'] !== undefined) {
            data = parameters['dto'];
        }

        return new Promise((resolve, reject) => {
            this.authenticate()
                .then(token => {
                    headers['Authorization'] = 'Bearer ' + token;
                    resolve(this.request('POST', domain + path, parameters, data, headers))
                })
        })
    };
    /**
     * 回答问题
     * @method
     * @name Qincai#ReplyQuestion
     * @param {object} parameters - method options and parameters
     * @param {string} parameters.id - 问题Id
     * @param {} parameters.dto - 新回答
     */
    ReplyQuestion(parameters) {
        if (parameters === undefined) {
            parameters = {};
        }
        let domain = this.domain,
            path = '/api/Question/{id}/Reply';
        let data = {},
            headers = {};

        headers['Accept'] = 'application/json';
        headers['Content-Type'] = 'application/json-patch+json,application/json,text/json,application/*+json';

        path = path.replace('{id}', parameters['id']);

        if (parameters['id'] === undefined) {
            return new Promise((resolve, reject) => {
                reject(new Error('Missing required  parameter: id'));
            });
        }

        if (parameters['dto'] !== undefined) {
            data = parameters['dto'];
        }

        return new Promise((resolve, reject) => {
            this.authenticate()
                .then(token => {
                    headers['Authorization'] = 'Bearer ' + token;
                    resolve(this.request('POST', domain + path, parameters, data, headers))
                })
        })
    };
    /**
     * 获取用户信息
     * @method
     * @name Qincai#GetUserInfoById
     * @param {object} parameters - method options and parameters
     * @param {string} parameters.id - 用户Id
     */
    GetUserInfoById(parameters) {
        if (parameters === undefined) {
            parameters = {};
        }
        let domain = this.domain,
            path = '/api/User/{id}';
        let data = {},
            headers = {};

        headers['Accept'] = 'application/json';
        headers['Content-Type'] = '';

        path = path.replace('{id}', parameters['id']);

        if (parameters['id'] === undefined) {
            return new Promise((resolve, reject) => {
                reject(new Error('Missing required  parameter: id'));
            });
        }

        return new Promise((resolve, reject) => {
            this.authenticate()
                .then(token => {
                    headers['Authorization'] = 'Bearer ' + token;
                    resolve(this.request('GET', domain + path, parameters, data, headers))
                })
        })
    };
    /**
     * 更新用户信息
     * @method
     * @name Qincai#UpdateUserInfo
     * @param {object} parameters - method options and parameters
     * @param {string} parameters.id - 
     * @param {} parameters.dto - 
     */
    UpdateUserInfo(parameters) {
        if (parameters === undefined) {
            parameters = {};
        }
        let domain = this.domain,
            path = '/api/User/{id}';
        let data = {},
            headers = {};

        headers['Accept'] = 'application/json';
        headers['Content-Type'] = 'application/json-patch+json,application/json,text/json,application/*+json';

        path = path.replace('{id}', parameters['id']);

        if (parameters['id'] === undefined) {
            return new Promise((resolve, reject) => {
                reject(new Error('Missing required  parameter: id'));
            });
        }

        if (parameters['dto'] !== undefined) {
            data = parameters['dto'];
        }

        return new Promise((resolve, reject) => {
            this.authenticate()
                .then(token => {
                    headers['Authorization'] = 'Bearer ' + token;
                    resolve(this.request('PUT', domain + path, parameters, data, headers))
                })
        })
    };
    /**
     * 获取用户自己的信息
     * @method
     * @name Qincai#GetMyUserInfo
     * @param {object} parameters - method options and parameters
     */
    GetMyUserInfo(parameters) {
        if (parameters === undefined) {
            parameters = {};
        }
        let domain = this.domain,
            path = '/api/User/me';
        let data = {},
            headers = {};

        headers['Accept'] = 'application/json';
        headers['Content-Type'] = '';

        return new Promise((resolve, reject) => {
            this.authenticate()
                .then(token => {
                    headers['Authorization'] = 'Bearer ' + token;
                    resolve(this.request('GET', domain + path, parameters, data, headers))
                })
        })
    };
    /**
     * 微信登录
     * @method
     * @name Qincai#WxLogin
     * @param {object} parameters - method options and parameters
     * @param {} parameters.dto - 登录参数
     */
    WxLogin(parameters) {
        if (parameters === undefined) {
            parameters = {};
        }
        let domain = this.domain,
            path = '/api/WxOpen/WxLogin';
        let data = {},
            headers = {};

        headers['Accept'] = 'application/json';
        headers['Content-Type'] = 'application/json-patch+json,application/json,text/json,application/*+json';

        if (parameters['dto'] !== undefined) {
            data = parameters['dto'];
        }

        return this.request('POST', domain + path, parameters, data, headers);
    };
    /**
     * 微信注册
     * @method
     * @name Qincai#WxRegister
     * @param {object} parameters - method options and parameters
     * @param {} parameters.dto - 注册参数
     */
    WxRegister(parameters) {
        if (parameters === undefined) {
            parameters = {};
        }
        let domain = this.domain,
            path = '/api/WxOpen/WxRegister';
        let data = {},
            headers = {};

        headers['Accept'] = 'application/json';
        headers['Content-Type'] = 'application/json-patch+json,application/json,text/json,application/*+json';

        if (parameters['dto'] !== undefined) {
            data = parameters['dto'];
        }

        return this.request('POST', domain + path, parameters, data, headers);
    };
    /**
     * 微信授权
     * @method
     * @name Qincai#WxAuthorize
     * @param {object} parameters - method options and parameters
     * @param {} parameters.dto - 登录态参数
     */
    WxAuthorize(parameters) {
        if (parameters === undefined) {
            parameters = {};
        }
        let domain = this.domain,
            path = '/api/WxOpen/WxAuthorize';
        let data = {},
            headers = {};

        headers['Accept'] = 'application/json';
        headers['Content-Type'] = 'application/json-patch+json,application/json,text/json,application/*+json';

        if (parameters['dto'] !== undefined) {
            data = parameters['dto'];
        }

        return this.request('POST', domain + path, parameters, data, headers);
    };
}