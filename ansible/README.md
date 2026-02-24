# setup ansible

content of ansible/host_vars/production_server.yml:
```yml
ansible_host: the ip address of the prod server
ansible_user: the user to log in as via ssh
ansible_ssh_private_key_file: path to the ssh key for the server
vault_db_password: password for mariadb server
```

also make sure the vault password is at
ansible/.vault_pass
