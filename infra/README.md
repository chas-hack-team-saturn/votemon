# setup terraform/tofu

create a vars file at infra/terraform.tfvars
and fill in the vars from variables.tf

then create a file named aws_creds.ini with this content filled int
```ini
[votemon]
aws_access_key_id = keyid
aws_secret_access_key = secret
```

