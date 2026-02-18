terraform {
  required_providers {
    aws = {
      source = "hashicorp/aws"
      version = "~> 5.0"
    }
    cloudflare = {
      source = "cloudflare/cloudflare"
      version = "~> 4.0"
    }
  }
}

provider "aws" {
  region                    = var.aws_region
  shared_credentials_files  = ["./aws_creds.ini"]
  profile                   = "votemon"
}

provider "cloudflare" {
  api_token = var.cloudflare_api_token
}

data "aws_caller_identity" "current" {}

# This prints the result to your terminal
output "aws_account_id" {
  value = data.aws_caller_identity.current.account_id
}

output "aws_user_arn" {
  value = data.aws_caller_identity.current.arn
}
